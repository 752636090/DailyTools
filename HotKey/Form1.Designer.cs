namespace HotKey
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.YouDaoTranslate = new System.Windows.Forms.Button();
            this.QQScreenShot = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // YouDaoTranslate
            // 
            this.YouDaoTranslate.Location = new System.Drawing.Point(0, -1);
            this.YouDaoTranslate.Name = "YouDaoTranslate";
            this.YouDaoTranslate.Size = new System.Drawing.Size(84, 29);
            this.YouDaoTranslate.TabIndex = 0;
            this.YouDaoTranslate.Text = "有道翻译";
            this.YouDaoTranslate.UseVisualStyleBackColor = true;
            this.YouDaoTranslate.Click += new System.EventHandler(this.button1_Click);
            // 
            // QQScreenShot
            // 
            this.QQScreenShot.Location = new System.Drawing.Point(90, -1);
            this.QQScreenShot.Name = "QQScreenShot";
            this.QQScreenShot.Size = new System.Drawing.Size(86, 29);
            this.QQScreenShot.TabIndex = 1;
            this.QQScreenShot.Text = "QQ截图";
            this.QQScreenShot.UseVisualStyleBackColor = true;
            this.QQScreenShot.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(182, -1);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(64, 29);
            this.button1.TabIndex = 2;
            this.button1.Text = "自定义";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(252, 1);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(125, 27);
            this.textBox1.TabIndex = 3;
            this.toolTip1.SetToolTip(this.textBox1, resources.GetString("textBox1.ToolTip"));
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // toolTip1
            // 
            this.toolTip1.Popup += new System.Windows.Forms.PopupEventHandler(this.toolTip1_Popup);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(384, -1);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(71, 29);
            this.button2.TabIndex = 4;
            this.button2.Text = "←说明";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(467, 28);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.QQScreenShot);
            this.Controls.Add(this.YouDaoTranslate);
            this.Name = "Form1";
            this.Text = "快捷键工具";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button YouDaoTranslate;
        private System.Windows.Forms.Button QQScreenShot;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button button2;
    }
}

