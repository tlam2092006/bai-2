using System.ComponentModel;
using System.Globalization;
using System.Text;

namespace bai2;

public sealed class LoanRecord
{
    public string LoanId { get; set; } = string.Empty;
    public string StudentId { get; set; } = string.Empty;
    public string StudentName { get; set; } = string.Empty;
    public string Equipment { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public DateTime BorrowDate { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? ActualReturnDate { get; set; }

    public string Status => ActualReturnDate.HasValue
        ? "Đã trả"
        : DueDate.Date < DateTime.Today ? "Quá hạn" : "Đang mượn";
}

public partial class Form1 : Form
{
    private readonly BindingList<LoanRecord> records = new();
    private readonly BindingList<LoanRecord> filteredRecords = new();
    private readonly BindingSource bindingSource = new();
    private LoanRecord? selectedRecord;

    public Form1()
    {
        InitializeComponent();
        ConfigureGrid();
        bindingSource.DataSource = filteredRecords;
        loanGrid.DataSource = bindingSource;
        equipmentComboBox.SelectedIndex = 0;
        borrowDatePicker.Value = DateTime.Today;
        dueDatePicker.Value = DateTime.Today.AddDays(7);
        returnDatePicker.Value = DateTime.Today;
        RefreshRecords();
    }

    private void ConfigureGrid()
    {
        loanGrid.AutoGenerateColumns = false;
        loanGrid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(LoanRecord.LoanId), HeaderText = "Mã phiếu", Width = 95 });
        loanGrid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(LoanRecord.StudentId), HeaderText = "Mã SV", Width = 90 });
        loanGrid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(LoanRecord.StudentName), HeaderText = "Họ tên", Width = 150 });
        loanGrid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(LoanRecord.Equipment), HeaderText = "Thiết bị", Width = 130 });
        loanGrid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(LoanRecord.Quantity), HeaderText = "SL", Width = 45 });
        loanGrid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(LoanRecord.BorrowDate), HeaderText = "Ngày mượn", DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy" }, Width = 95 });
        loanGrid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(LoanRecord.DueDate), HeaderText = "Hạn trả", DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy" }, Width = 95 });
        loanGrid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(LoanRecord.ActualReturnDate), HeaderText = "Ngày trả", DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy" }, Width = 95 });
        loanGrid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = nameof(LoanRecord.Status), HeaderText = "Tình trạng", Width = 95 });
    }

    private void RefreshRecords()
    {
        filteredRecords.RaiseListChangedEvents = false;
        filteredRecords.Clear();
        string keyword = searchTextBox.Text.Trim();
        string status = statusComboBox.Text;
        foreach (LoanRecord record in records.Where(r =>
                     (keyword.Length == 0 || r.StudentId.Contains(keyword, StringComparison.OrdinalIgnoreCase) || r.Equipment.Contains(keyword, StringComparison.OrdinalIgnoreCase)) &&
                     (status == "Tất cả" || r.Status == status)))
        {
            filteredRecords.Add(record);
        }
        filteredRecords.RaiseListChangedEvents = true;
        bindingSource.ResetBindings(false);
        totalLabel.Text = $"Tổng số phiếu: {records.Count}";
        overdueLabel.Text = $"Quá hạn: {records.Count(r => r.Status == "Quá hạn")}";
    }

    private void ClearInputs()
    {
        selectedRecord = null;
        loanIdTextBox.Clear();
        studentIdTextBox.Clear();
        studentNameTextBox.Clear();
        quantityNumeric.Value = 1;
        borrowDatePicker.Value = DateTime.Today;
        dueDatePicker.Value = DateTime.Today.AddDays(7);
        returnDatePicker.Value = DateTime.Today;
        returnedCheckBox.Checked = false;
        errorProvider.Clear();
        loanGrid.ClearSelection();
        loanIdTextBox.Focus();
    }

    private LoanRecord ReadInputs()
    {
        return new LoanRecord
        {
            LoanId = loanIdTextBox.Text.Trim(), StudentId = studentIdTextBox.Text.Trim(),
            StudentName = studentNameTextBox.Text.Trim(), Equipment = equipmentComboBox.Text.Trim(),
            Quantity = (int)quantityNumeric.Value, BorrowDate = borrowDatePicker.Value.Date,
            DueDate = dueDatePicker.Value.Date, ActualReturnDate = returnedCheckBox.Checked ? returnDatePicker.Value.Date : null
        };
    }

    private bool ValidateRecord(LoanRecord record, bool updating)
    {
        errorProvider.Clear();
        bool valid = true;
        if (string.IsNullOrWhiteSpace(record.LoanId)) { errorProvider.SetError(loanIdTextBox, "Mã phiếu không được để trống."); valid = false; }
        else if (records.Any(r => r != selectedRecord && r.LoanId.Equals(record.LoanId, StringComparison.OrdinalIgnoreCase))) { errorProvider.SetError(loanIdTextBox, "Mã phiếu đã tồn tại."); valid = false; }
        if (string.IsNullOrWhiteSpace(record.StudentId)) { errorProvider.SetError(studentIdTextBox, "Mã sinh viên không được để trống."); valid = false; }
        if (string.IsNullOrWhiteSpace(record.Equipment)) { errorProvider.SetError(equipmentComboBox, "Thiết bị không được để trống."); valid = false; }
        if (record.Quantity <= 0) { errorProvider.SetError(quantityNumeric, "Số lượng phải lớn hơn 0."); valid = false; }
        if (record.DueDate < record.BorrowDate) { errorProvider.SetError(dueDatePicker, "Hạn trả không được trước ngày mượn."); valid = false; }
        if (record.ActualReturnDate.HasValue && record.ActualReturnDate.Value < record.BorrowDate) { errorProvider.SetError(returnDatePicker, "Ngày trả không được trước ngày mượn."); valid = false; }
        return valid;
    }

    private void AddButton_Click(object? sender, EventArgs e)
    {
        LoanRecord record = ReadInputs();
        if (!ValidateRecord(record, false)) return;
        records.Add(record); RefreshRecords(); ClearInputs();
    }

    private void UpdateButton_Click(object? sender, EventArgs e)
    {
        if (selectedRecord is null) { MessageBox.Show("Hãy chọn một phiếu để cập nhật.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information); return; }
        LoanRecord input = ReadInputs();
        if (!ValidateRecord(input, true)) return;
        selectedRecord.LoanId = input.LoanId; selectedRecord.StudentId = input.StudentId; selectedRecord.StudentName = input.StudentName;
        selectedRecord.Equipment = input.Equipment; selectedRecord.Quantity = input.Quantity; selectedRecord.BorrowDate = input.BorrowDate;
        selectedRecord.DueDate = input.DueDate; selectedRecord.ActualReturnDate = input.ActualReturnDate;
        RefreshRecords(); ClearInputs();
    }

    private void DeleteButton_Click(object? sender, EventArgs e)
    {
        if (selectedRecord is null) return;
        if (MessageBox.Show("Bạn có chắc muốn xóa phiếu này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
        { records.Remove(selectedRecord); RefreshRecords(); ClearInputs(); }
    }

    private void MarkReturnedButton_Click(object? sender, EventArgs e)
    {
        if (selectedRecord is null) { MessageBox.Show("Hãy chọn một phiếu đang mượn.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information); return; }
        if (selectedRecord.ActualReturnDate.HasValue) return;
        selectedRecord.ActualReturnDate = DateTime.Today;
        LoadRecord(selectedRecord); RefreshRecords();
    }

    private void LoadRecord(LoanRecord record)
    {
        selectedRecord = record; loanIdTextBox.Text = record.LoanId; studentIdTextBox.Text = record.StudentId;
        studentNameTextBox.Text = record.StudentName; equipmentComboBox.Text = record.Equipment; quantityNumeric.Value = record.Quantity;
        borrowDatePicker.Value = record.BorrowDate; dueDatePicker.Value = record.DueDate;
        returnedCheckBox.Checked = record.ActualReturnDate.HasValue; returnDatePicker.Value = record.ActualReturnDate ?? DateTime.Today;
        errorProvider.Clear();
    }

    private void LoanGrid_SelectionChanged(object? sender, EventArgs e)
    { if (loanGrid.CurrentRow?.DataBoundItem is LoanRecord record) LoadRecord(record); }

    private void ExportButton_Click(object? sender, EventArgs e)
    {
        using SaveFileDialog dialog = new() { Filter = "CSV file (*.csv)|*.csv", FileName = "phieu_muon.csv", Title = "Xuất danh sách phiếu" };
        if (dialog.ShowDialog() != DialogResult.OK) return;
        StringBuilder csv = new(); csv.AppendLine("Mã phiếu,Mã sinh viên,Họ tên,Thiết bị,Số lượng,Ngày mượn,Hạn trả,Ngày trả,Tình trạng");
        foreach (LoanRecord r in filteredRecords) csv.AppendLine(string.Join(',', new[] { r.LoanId, r.StudentId, r.StudentName, r.Equipment, r.Quantity.ToString(CultureInfo.InvariantCulture), r.BorrowDate.ToString("dd/MM/yyyy"), r.DueDate.ToString("dd/MM/yyyy"), r.ActualReturnDate?.ToString("dd/MM/yyyy") ?? "", r.Status }.Select(EscapeCsv)));
        File.WriteAllText(dialog.FileName, csv.ToString(), Encoding.UTF8);
        MessageBox.Show("Đã xuất dữ liệu.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private static string EscapeCsv(string value) => $"\"{value.Replace("\"", "\"\"")}\"";
    private void SearchTextBox_TextChanged(object? sender, EventArgs e) => RefreshRecords();
    private void StatusComboBox_SelectedIndexChanged(object? sender, EventArgs e) => RefreshRecords();
    private void ClearButton_Click(object? sender, EventArgs e) => ClearInputs();
}
