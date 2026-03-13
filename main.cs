using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace FoodTip;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new FoodTipForm());
    }
}

internal sealed class FoodTipForm : Form
{
    private readonly TextBox foodCostTextBox;
    private readonly TextBox tipPercentTextBox;
    private readonly TextBox resultTextBox;

    internal FoodTipForm()
    {
        Font uiFont = new("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point);

        AutoScaleMode = AutoScaleMode.Font;
        BackColor = SystemColors.Control;
        ClientSize = new Size(620, 360);
        Font = uiFont;
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        MinimizeBox = false;
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Food Tip Calculator";

        Label foodCostLabel = new()
        {
            AutoSize = true,
            Location = new Point(24, 32),
            Text = "ค่าอาหาร"
        };

        foodCostTextBox = new TextBox
        {
            Location = new Point(200, 28),
            Size = new Size(280, 32)
        };

        Label tipPercentLabel = new()
        {
            AutoSize = true,
            Location = new Point(24, 82),
            Text = "ทิป (%)"
        };

        tipPercentTextBox = new TextBox
        {
            Location = new Point(200, 78),
            Size = new Size(280, 32)
        };

        resultTextBox = new TextBox
        {
            Location = new Point(24, 140),
            Multiline = true,
            ReadOnly = true,
            ScrollBars = ScrollBars.Vertical,
            Size = new Size(572, 150)
        };

        Button calculateButton = new()
        {
            Location = new Point(170, 308),
            Size = new Size(120, 38),
            Text = "คำนวณ"
        };
        calculateButton.Click += CalculateButton_Click;

        Button resetButton = new()
        {
            Location = new Point(320, 308),
            Size = new Size(120, 38),
            Text = "Reset"
        };
        resetButton.Click += ResetButton_Click;

        Controls.Add(foodCostLabel);
        Controls.Add(foodCostTextBox);
        Controls.Add(tipPercentLabel);
        Controls.Add(tipPercentTextBox);
        Controls.Add(resultTextBox);
        Controls.Add(calculateButton);
        Controls.Add(resetButton);

        AcceptButton = calculateButton;
        CancelButton = resetButton;

        ResetFields();
    }

    private void CalculateButton_Click(object? sender, EventArgs e)
    {
        if (!TryParseDecimal(foodCostTextBox.Text, out decimal foodCost) || foodCost < 0)
        {
            ShowValidationError("กรุณากรอกค่าอาหารเป็นตัวเลขที่มากกว่าหรือเท่ากับ 0", foodCostTextBox);
            return;
        }

        if (!TryParseDecimal(tipPercentTextBox.Text, out decimal tipPercent) || tipPercent < 0)
        {
            ShowValidationError("กรุณากรอกเปอร์เซ็นต์ทิปเป็นตัวเลขที่มากกว่าหรือเท่ากับ 0", tipPercentTextBox);
            return;
        }

        decimal tipAmount = foodCost * tipPercent / 100m;
        decimal total = foodCost + tipAmount;

        resultTextBox.Text =
            $"ค่าอาหาร: {foodCost:N2} บาท{Environment.NewLine}" +
            $"เปอร์เซ็นต์ทิป: {tipPercent:N2}%{Environment.NewLine}" +
            $"ค่าทิป: {tipAmount:N2} บาท{Environment.NewLine}{Environment.NewLine}" +
            $"รวมทั้งหมด: {total:N2} บาท";
    }

    private void ResetButton_Click(object? sender, EventArgs e)
    {
        ResetFields();
    }

    private void ResetFields()
    {
        foodCostTextBox.Text = string.Empty;
        tipPercentTextBox.Text = string.Empty;
        resultTextBox.Text = "กรอกค่าอาหารและเปอร์เซ็นต์ทิป แล้วกดปุ่ม คำนวณ";
        foodCostTextBox.Focus();
    }

    private static bool TryParseDecimal(string? value, out decimal result)
    {
        return decimal.TryParse(value, NumberStyles.Number, CultureInfo.CurrentCulture, out result) ||
               decimal.TryParse(value, NumberStyles.Number, CultureInfo.InvariantCulture, out result);
    }

    private void ShowValidationError(string message, Control target)
    {
        MessageBox.Show(this, message, "ข้อมูลไม่ถูกต้อง", MessageBoxButtons.OK, MessageBoxIcon.Error);
        target.Focus();
        if (target is TextBox textBox)
        {
            textBox.SelectAll();
        }
    }
}
