namespace LiveSplit.Semblance {
	partial class SplitterSettings {
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.btnAddSplit = new System.Windows.Forms.Button();
			this.flowMain = new System.Windows.Forms.FlowLayoutPanel();
			this.flowOptions = new System.Windows.Forms.FlowLayoutPanel();
			this.lblDefaultSplits = new System.Windows.Forms.Label();
			this.btnWorldSplits = new System.Windows.Forms.Button();
			this.btnLevels = new System.Windows.Forms.Button();
			this.flowMain.SuspendLayout();
			this.flowOptions.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnAddSplit
			// 
			this.btnAddSplit.AutoSize = true;
			this.btnAddSplit.Location = new System.Drawing.Point(3, 3);
			this.btnAddSplit.Name = "btnAddSplit";
			this.btnAddSplit.Size = new System.Drawing.Size(59, 23);
			this.btnAddSplit.TabIndex = 0;
			this.btnAddSplit.Text = "Add Split";
			this.btnAddSplit.UseVisualStyleBackColor = true;
			this.btnAddSplit.Click += new System.EventHandler(this.btnAddSplit_Click);
			// 
			// flowMain
			// 
			this.flowMain.AllowDrop = true;
			this.flowMain.AutoSize = true;
			this.flowMain.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.flowMain.Controls.Add(this.flowOptions);
			this.flowMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.flowMain.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.flowMain.Location = new System.Drawing.Point(0, 0);
			this.flowMain.Margin = new System.Windows.Forms.Padding(0);
			this.flowMain.Name = "flowMain";
			this.flowMain.Size = new System.Drawing.Size(227, 29);
			this.flowMain.TabIndex = 0;
			this.flowMain.WrapContents = false;
			this.flowMain.DragDrop += new System.Windows.Forms.DragEventHandler(this.flowMain_DragDrop);
			this.flowMain.DragEnter += new System.Windows.Forms.DragEventHandler(this.flowMain_DragEnter);
			this.flowMain.DragOver += new System.Windows.Forms.DragEventHandler(this.flowMain_DragOver);
			// 
			// flowOptions
			// 
			this.flowOptions.AutoSize = true;
			this.flowOptions.Controls.Add(this.btnAddSplit);
			this.flowOptions.Controls.Add(this.lblDefaultSplits);
			this.flowOptions.Controls.Add(this.btnWorldSplits);
			this.flowOptions.Controls.Add(this.btnLevels);
			this.flowOptions.Location = new System.Drawing.Point(0, 0);
			this.flowOptions.Margin = new System.Windows.Forms.Padding(0);
			this.flowOptions.Name = "flowOptions";
			this.flowOptions.Size = new System.Drawing.Size(227, 29);
			this.flowOptions.TabIndex = 0;
			// 
			// lblDefaultSplits
			// 
			this.lblDefaultSplits.Location = new System.Drawing.Point(68, 0);
			this.lblDefaultSplits.Name = "lblDefaultSplits";
			this.lblDefaultSplits.Size = new System.Drawing.Size(44, 29);
			this.lblDefaultSplits.TabIndex = 3;
			this.lblDefaultSplits.Text = "Default:";
			this.lblDefaultSplits.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// btnWorldSplits
			// 
			this.btnWorldSplits.AutoSize = true;
			this.btnWorldSplits.Location = new System.Drawing.Point(118, 3);
			this.btnWorldSplits.Name = "btnWorldSplits";
			this.btnWorldSplits.Size = new System.Drawing.Size(50, 23);
			this.btnWorldSplits.TabIndex = 1;
			this.btnWorldSplits.Text = "Worlds";
			this.btnWorldSplits.UseVisualStyleBackColor = true;
			this.btnWorldSplits.Click += new System.EventHandler(this.btnWorlds_Click);
			// 
			// btnLevels
			// 
			this.btnLevels.AutoSize = true;
			this.btnLevels.Location = new System.Drawing.Point(174, 3);
			this.btnLevels.Name = "btnLevels";
			this.btnLevels.Size = new System.Drawing.Size(50, 23);
			this.btnLevels.TabIndex = 2;
			this.btnLevels.Text = "Levels";
			this.btnLevels.UseVisualStyleBackColor = true;
			this.btnLevels.Click += new System.EventHandler(this.btnLevels_Click);
			// 
			// SplitterSettings
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoScroll = true;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.Controls.Add(this.flowMain);
			this.Margin = new System.Windows.Forms.Padding(0);
			this.Name = "SplitterSettings";
			this.Size = new System.Drawing.Size(227, 29);
			this.Load += new System.EventHandler(this.Settings_Load);
			this.flowMain.ResumeLayout(false);
			this.flowMain.PerformLayout();
			this.flowOptions.ResumeLayout(false);
			this.flowOptions.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.Button btnAddSplit;
		private System.Windows.Forms.FlowLayoutPanel flowMain;
		private System.Windows.Forms.FlowLayoutPanel flowOptions;
		private System.Windows.Forms.Button btnWorldSplits;
		private System.Windows.Forms.Label lblDefaultSplits;
		private System.Windows.Forms.Button btnLevels;
	}
}
