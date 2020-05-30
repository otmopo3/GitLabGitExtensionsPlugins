namespace GitExtGitLabBuildServerPlugin.Settings
{
	partial class GitlabSettingsUserControl
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
			this.GitlabAddressTb = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.GitlabKeyTb = new System.Windows.Forms.TextBox();
			this.DefaultProjectIdTb = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// GitlabAddressTb
			// 
			this.GitlabAddressTb.Location = new System.Drawing.Point(148, 15);
			this.GitlabAddressTb.Name = "GitlabAddressTb";
			this.GitlabAddressTb.Size = new System.Drawing.Size(100, 23);
			this.GitlabAddressTb.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(19, 23);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(83, 15);
			this.label1.TabIndex = 1;
			this.label1.Text = "Gitlab Address";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(19, 67);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(60, 15);
			this.label2.TabIndex = 2;
			this.label2.Text = "Gitlab Key";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(19, 107);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(98, 15);
			this.label3.TabIndex = 3;
			this.label3.Text = "Default Project Id";
			// 
			// GitlabKeyTb
			// 
			this.GitlabKeyTb.Location = new System.Drawing.Point(148, 59);
			this.GitlabKeyTb.Name = "GitlabKeyTb";
			this.GitlabKeyTb.Size = new System.Drawing.Size(100, 23);
			this.GitlabKeyTb.TabIndex = 0;
			// 
			// DefaultProjectIdTb
			// 
			this.DefaultProjectIdTb.Location = new System.Drawing.Point(148, 99);
			this.DefaultProjectIdTb.Name = "DefaultProjectIdTb";
			this.DefaultProjectIdTb.Size = new System.Drawing.Size(100, 23);
			this.DefaultProjectIdTb.TabIndex = 0;
			// 
			// GitlabSettingsUserControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.DefaultProjectIdTb);
			this.Controls.Add(this.GitlabKeyTb);
			this.Controls.Add(this.GitlabAddressTb);
			this.Name = "GitlabSettingsUserControl";
			this.Size = new System.Drawing.Size(800, 450);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox GitlabAddressTb;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox GitlabKeyTb;
		private System.Windows.Forms.TextBox DefaultProjectIdTb;
	}
}