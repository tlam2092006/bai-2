namespace bai2;

partial class Form1
{
    private System.ComponentModel.IContainer? components = null;
    private TableLayoutPanel mainLayout = null!;
    private TableLayoutPanel inputLayout = null!;
    private Label loanIdLabel = null!, studentIdLabel = null!, studentNameLabel = null!, equipmentLabel = null!, quantityLabel = null!, borrowDateLabel = null!, dueDateLabel = null!, returnDateLabel = null!;
    private TextBox loanIdTextBox = null!, studentIdTextBox = null!, studentNameTextBox = null!, searchTextBox = null!;
    private ComboBox equipmentComboBox = null!, statusComboBox = null!;
    private NumericUpDown quantityNumeric = null!;
    private DateTimePicker borrowDatePicker = null!, dueDatePicker = null!, returnDatePicker = null!;
    private CheckBox returnedCheckBox = null!;
    private Button addButton = null!, updateButton = null!, deleteButton = null!, clearButton = null!, markReturnedButton = null!, exportButton = null!;
    private DataGridView loanGrid = null!;
    private ErrorProvider errorProvider = null!;
    private StatusStrip statusStrip = null!;
    private ToolStripStatusLabel totalLabel = null!, overdueLabel = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing) components?.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        mainLayout = new TableLayoutPanel(); inputLayout = new TableLayoutPanel();
        loanIdLabel = new Label(); studentIdLabel = new Label(); studentNameLabel = new Label(); equipmentLabel = new Label(); quantityLabel = new Label(); borrowDateLabel = new Label(); dueDateLabel = new Label(); returnDateLabel = new Label();
        loanIdTextBox = new TextBox(); studentIdTextBox = new TextBox(); studentNameTextBox = new TextBox(); searchTextBox = new TextBox();
        equipmentComboBox = new ComboBox(); statusComboBox = new ComboBox(); quantityNumeric = new NumericUpDown();
        borrowDatePicker = new DateTimePicker(); dueDatePicker = new DateTimePicker(); returnDatePicker = new DateTimePicker(); returnedCheckBox = new CheckBox();
        addButton = new Button(); updateButton = new Button(); deleteButton = new Button(); clearButton = new Button(); markReturnedButton = new Button(); exportButton = new Button();
        loanGrid = new DataGridView(); errorProvider = new ErrorProvider(components); statusStrip = new StatusStrip(); totalLabel = new ToolStripStatusLabel(); overdueLabel = new ToolStripStatusLabel();
        ((System.ComponentModel.ISupportInitialize)quantityNumeric).BeginInit(); ((System.ComponentModel.ISupportInitialize)loanGrid).BeginInit();
        SuspendLayout();
        Text = "Lab Equipment Checkout"; StartPosition = FormStartPosition.CenterScreen; MinimumSize = new Size(900, 600); ClientSize = new Size(1200, 720);
        mainLayout.Dock = DockStyle.Fill; mainLayout.Padding = new Padding(10); mainLayout.RowCount = 5; mainLayout.ColumnCount = 1;
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 155)); mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 42)); mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 35)); mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100)); mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 25));
        inputLayout.Dock = DockStyle.Fill; inputLayout.ColumnCount = 6; inputLayout.RowCount = 3; inputLayout.Padding = new Padding(0, 0, 0, 5);
        for (int i = 0; i < 6; i++) inputLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, i % 2 == 0 ? 10 : 23));
        inputLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 33)); inputLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 33)); inputLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 34));
        ConfigureLabel(loanIdLabel, "Mã phiếu:"); ConfigureLabel(studentIdLabel, "Mã sinh viên:"); ConfigureLabel(studentNameLabel, "Họ tên:"); ConfigureLabel(equipmentLabel, "Thiết bị:"); ConfigureLabel(quantityLabel, "Số lượng:"); ConfigureLabel(borrowDateLabel, "Ngày mượn:"); ConfigureLabel(dueDateLabel, "Hạn trả:"); ConfigureLabel(returnDateLabel, "Ngày trả:");
        AddInput(loanIdLabel, loanIdTextBox, 0, 0); AddInput(studentIdLabel, studentIdTextBox, 2, 0); AddInput(studentNameLabel, studentNameTextBox, 4, 0);
        AddInput(equipmentLabel, equipmentComboBox, 0, 1); AddInput(quantityLabel, quantityNumeric, 2, 1); AddInput(borrowDateLabel, borrowDatePicker, 4, 1);
        AddInput(dueDateLabel, dueDatePicker, 0, 2); AddInput(returnDateLabel, returnDatePicker, 2, 2);
        returnedCheckBox.Text = "Đã trả"; returnedCheckBox.Dock = DockStyle.Fill; returnedCheckBox.TextAlign = ContentAlignment.MiddleLeft; inputLayout.Controls.Add(returnedCheckBox, 4, 2);
        equipmentComboBox.Items.AddRange(new object[] { "Laptop", "Máy chiếu", "Arduino", "Raspberry Pi", "Bộ cảm biến" }); equipmentComboBox.DropDownStyle = ComboBoxStyle.DropDownList; equipmentComboBox.Dock = DockStyle.Fill;
        quantityNumeric.Minimum = 1; quantityNumeric.Maximum = 1000; quantityNumeric.Dock = DockStyle.Fill;
        foreach (DateTimePicker picker in new[] { borrowDatePicker, dueDatePicker, returnDatePicker }) { picker.Format = DateTimePickerFormat.Short; picker.Dock = DockStyle.Fill; }
        mainLayout.Controls.Add(inputLayout, 0, 0);
        FlowLayoutPanel actions = new() { Dock = DockStyle.Fill, FlowDirection = FlowDirection.LeftToRight, WrapContents = false };
        ConfigureButton(addButton, "Thêm", AddButton_Click); ConfigureButton(updateButton, "Cập nhật", UpdateButton_Click); ConfigureButton(deleteButton, "Xóa", DeleteButton_Click); ConfigureButton(clearButton, "Làm mới", ClearButton_Click); ConfigureButton(markReturnedButton, "Đánh dấu đã trả", MarkReturnedButton_Click); ConfigureButton(exportButton, "Xuất CSV", ExportButton_Click);
        actions.Controls.AddRange(new Control[] { addButton, updateButton, deleteButton, clearButton, markReturnedButton, exportButton }); mainLayout.Controls.Add(actions, 0, 1);
        TableLayoutPanel filterLayout = new() { Dock = DockStyle.Fill, ColumnCount = 4, RowCount = 1 };
        filterLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130)); filterLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60)); filterLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100)); filterLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));
        filterLayout.Controls.Add(new Label { Text = "Tìm mã SV/thiết bị:", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft }, 0, 0); filterLayout.Controls.Add(searchTextBox, 1, 0); filterLayout.Controls.Add(new Label { Text = "Lọc trạng thái:", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft }, 2, 0); filterLayout.Controls.Add(statusComboBox, 3, 0);
        searchTextBox.Dock = DockStyle.Fill; searchTextBox.TextChanged += SearchTextBox_TextChanged; statusComboBox.Dock = DockStyle.Fill; statusComboBox.DropDownStyle = ComboBoxStyle.DropDownList; statusComboBox.Items.AddRange(new object[] { "Tất cả", "Đang mượn", "Quá hạn", "Đã trả" }); statusComboBox.SelectedIndex = 0; statusComboBox.SelectedIndexChanged += StatusComboBox_SelectedIndexChanged; mainLayout.Controls.Add(filterLayout, 0, 2);
        loanGrid.Dock = DockStyle.Fill; loanGrid.AllowUserToAddRows = false; loanGrid.AllowUserToDeleteRows = false; loanGrid.ReadOnly = true; loanGrid.MultiSelect = false; loanGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect; loanGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None; loanGrid.SelectionChanged += LoanGrid_SelectionChanged; mainLayout.Controls.Add(loanGrid, 0, 3);
        statusStrip.Items.AddRange(new ToolStripItem[] { totalLabel, overdueLabel }); totalLabel.Spring = true; totalLabel.TextAlign = ContentAlignment.MiddleLeft; overdueLabel.TextAlign = ContentAlignment.MiddleRight; mainLayout.Controls.Add(statusStrip, 0, 4);
        Controls.Add(mainLayout); ((System.ComponentModel.ISupportInitialize)quantityNumeric).EndInit(); ((System.ComponentModel.ISupportInitialize)loanGrid).EndInit(); ResumeLayout(false); PerformLayout();
    }

    private static void ConfigureLabel(Label label, string text) { label.Text = text; label.Dock = DockStyle.Fill; label.TextAlign = ContentAlignment.MiddleLeft; }
    private void AddInput(Control label, Control input, int column, int row) { input.Dock = DockStyle.Fill; input.Margin = new Padding(3); inputLayout.Controls.Add(label, column, row); inputLayout.Controls.Add(input, column + 1, row); }
    private static void ConfigureButton(Button button, string text, EventHandler handler) { button.Text = text; button.AutoSize = true; button.Height = 30; button.Click += handler; }
}