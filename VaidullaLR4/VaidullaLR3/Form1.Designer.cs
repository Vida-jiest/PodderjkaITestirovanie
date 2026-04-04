namespace VaidullaLR4
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.textBoxNumerator = new System.Windows.Forms.TextBox();
            this.textBoxDenominator = new System.Windows.Forms.TextBox();
            this.labelResult = new System.Windows.Forms.Label();
            this.labelStatus = new System.Windows.Forms.Label();
            this.buttonDivide = new System.Windows.Forms.Button();
            this.labelNumerator = new System.Windows.Forms.Label();
            this.labelDenominator = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelNumerator
            // 
            this.labelNumerator.AutoSize = true;
            this.labelNumerator.Location = new System.Drawing.Point(13, 15);
            this.labelNumerator.Name = "labelNumerator";
            this.labelNumerator.Size = new System.Drawing.Size(59, 13);
            this.labelNumerator.TabIndex = 0;
            this.labelNumerator.Text = "Числитель:";
            // 
            // textBoxNumerator
            // 
            this.textBoxNumerator.Location = new System.Drawing.Point(78, 12);
            this.textBoxNumerator.Name = "textBoxNumerator";
            this.textBoxNumerator.Size = new System.Drawing.Size(100, 20);
            this.textBoxNumerator.TabIndex = 1;
            // 
            // labelDenominator
            // 
            this.labelDenominator.AutoSize = true;
            this.labelDenominator.Location = new System.Drawing.Point(184, 15);
            this.labelDenominator.Name = "labelDenominator";
            this.labelDenominator.Size = new System.Drawing.Size(75, 13);
            this.labelDenominator.TabIndex = 2;
            this.labelDenominator.Text = "Знаменатель:";
            // 
            // textBoxDenominator
            // 
            this.textBoxDenominator.Location = new System.Drawing.Point(265, 12);
            this.textBoxDenominator.Name = "textBoxDenominator";
            this.textBoxDenominator.Size = new System.Drawing.Size(100, 20);
            this.textBoxDenominator.TabIndex = 3;
            // 
            // buttonDivide
            // 
            this.buttonDivide.Location = new System.Drawing.Point(150, 50);
            this.buttonDivide.Name = "buttonDivide";
            this.buttonDivide.Size = new System.Drawing.Size(100, 30);
            this.buttonDivide.TabIndex = 4;
            this.buttonDivide.Text = "Разделить";
            this.buttonDivide.UseVisualStyleBackColor = true;
            this.buttonDivide.Click += new System.EventHandler(this.buttonDivide_Click);
            // 
            // labelResult
            // 
            this.labelResult.AutoSize = true;
            this.labelResult.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelResult.Location = new System.Drawing.Point(13, 100);
            this.labelResult.Name = "labelResult";
            this.labelResult.Size = new System.Drawing.Size(60, 15);
            this.labelResult.TabIndex = 5;
            this.labelResult.Text = "Результат:";
            // 
            // labelStatus
            // 
            this.labelStatus.AutoSize = true;
            this.labelStatus.Location = new System.Drawing.Point(13, 130);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(70, 13);
            this.labelStatus.TabIndex = 6;
            this.labelStatus.Text = "Готов к работе";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 161);
            this.Controls.Add(this.labelStatus);
            this.Controls.Add(this.labelResult);
            this.Controls.Add(this.buttonDivide);
            this.Controls.Add(this.textBoxDenominator);
            this.Controls.Add(this.labelDenominator);
            this.Controls.Add(this.textBoxNumerator);
            this.Controls.Add(this.labelNumerator);
            this.Name = "Form1";
            this.Text = "Калькулятор деления";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.TextBox textBoxNumerator;
        private System.Windows.Forms.TextBox textBoxDenominator;
        private System.Windows.Forms.Label labelResult;
        private System.Windows.Forms.Label labelStatus;
        private System.Windows.Forms.Button buttonDivide;
        private System.Windows.Forms.Label labelNumerator;
        private System.Windows.Forms.Label labelDenominator;
    }
}