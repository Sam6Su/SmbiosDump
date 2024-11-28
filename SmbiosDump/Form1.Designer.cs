
namespace SmbiosDump
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Rtb_Data = new System.Windows.Forms.RichTextBox();
            this.Lb_Types = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Rtb_Data
            // 
            this.Rtb_Data.Location = new System.Drawing.Point(327, 12);
            this.Rtb_Data.Name = "Rtb_Data";
            this.Rtb_Data.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
            this.Rtb_Data.Size = new System.Drawing.Size(1025, 797);
            this.Rtb_Data.TabIndex = 0;
            this.Rtb_Data.Text = "";
            // 
            // Lb_Types
            // 
            this.Lb_Types.FormattingEnabled = true;
            this.Lb_Types.Items.AddRange(new object[] {
            "[SMBIOS Entry]"});
            this.Lb_Types.Location = new System.Drawing.Point(12, 12);
            this.Lb_Types.Name = "Lb_Types";
            this.Lb_Types.Size = new System.Drawing.Size(309, 797);
            this.Lb_Types.TabIndex = 1;
            this.Lb_Types.SelectedIndexChanged += new System.EventHandler(this.Lb_Types_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1228, 796);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Powered By Sam Su";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1364, 818);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Lb_Types);
            this.Controls.Add(this.Rtb_Data);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1380, 857);
            this.Name = "Form1";
            this.ShowIcon = false;
            this.Text = "SMBIOS decoder";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox Rtb_Data;
        private System.Windows.Forms.ListBox Lb_Types;
        private System.Windows.Forms.Label label1;
    }
}

