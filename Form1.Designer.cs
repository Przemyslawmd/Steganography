namespace Stegan
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.menuStripOne = new System.Windows.Forms.MenuStrip();
            this.fileMenuStripOne = new System.Windows.Forms.ToolStripMenuItem();
            this.menuOpenGraphic = new System.Windows.Forms.ToolStripMenuItem();           
            this.menuOpenFile = new System.Windows.Forms.ToolStripMenuItem();            
            this.menuSaveGraphic = new System.Windows.Forms.ToolStripMenuItem();            
            this.menuSaveData = new System.Windows.Forms.ToolStripMenuItem();            
            this.menuRemoveGraphic = new System.Windows.Forms.ToolStripMenuItem();            
            this.menuRemoveData = new System.Windows.Forms.ToolStripMenuItem();            
            this.menuClearText = new System.Windows.Forms.ToolStripMenuItem();
            this.actionMenuStripOne = new System.Windows.Forms.ToolStripMenuItem();
            this.menuCoverText = new System.Windows.Forms.ToolStripMenuItem();            
            this.menuCoverFile = new System.Windows.Forms.ToolStripMenuItem();            
            this.menuDiscoverText = new System.Windows.Forms.ToolStripMenuItem();            
            this.menuUncoverFile = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsMenuStripOne = new System.Windows.Forms.ToolStripMenuItem();
            this.infoMenuStrip = new System.Windows.Forms.ToolStripMenuItem();                   
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.textControl = new System.Windows.Forms.TextBox();
            this.fileNameControl = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.menuStripOne.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // MENU MAIN STRIP 
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////// 
            
            this.menuStripOne.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileMenuStripOne,
            this.actionMenuStripOne,
            this.settingsMenuStripOne,
            this.infoMenuStrip});
            this.menuStripOne.Location = new System.Drawing.Point(0, 0);            
            this.menuStripOne.Size = new System.Drawing.Size(1018, 24);
            this.menuStripOne.TabIndex = 0;
                       
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////// 
            // MENU FILE
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////// 
            
            this.fileMenuStripOne.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuOpenGraphic,            
            new System.Windows.Forms.ToolStripSeparator(),
            this.menuOpenFile,                
            new System.Windows.Forms.ToolStripSeparator(),
            this.menuSaveGraphic,
            new System.Windows.Forms.ToolStripSeparator(),
            this.menuSaveData,
            new System.Windows.Forms.ToolStripSeparator(),
            this.menuRemoveGraphic,
            new System.Windows.Forms.ToolStripSeparator(),
            this.menuRemoveData,
            new System.Windows.Forms.ToolStripSeparator(),
            this.menuClearText});            
            this.fileMenuStripOne.Size = new System.Drawing.Size(12, 20);            
           
            // MENU OPEN GRAPHIC             
            
            this.menuOpenGraphic.Size = new System.Drawing.Size(67, 22);
            this.menuOpenGraphic.Click += new System.EventHandler(this.openGraphicFile);              
             
            // MENU OPEN FILE             
                        
            this.menuOpenFile.Size = new System.Drawing.Size(67, 22);
            this.menuOpenFile.Click += new System.EventHandler(this.openFile);                   
           
            // MENU SAVE GRAPHIC
            
            this.menuSaveGraphic.Enabled = false;
            this.menuSaveGraphic.ShowShortcutKeys = false;
            this.menuSaveGraphic.Size = new System.Drawing.Size(67, 22);
            this.menuSaveGraphic.Click += new System.EventHandler(this.saveGraphic);                  
             
            // MENU SAVE DATA
             
            this.menuSaveData.Enabled = false;            
            this.menuSaveData.Size = new System.Drawing.Size(67, 22);
            this.menuSaveData.Click += new System.EventHandler(this.saveUncoveredData);            
             
            // MENU REMOVE GRAPHIC
             
            this.menuRemoveGraphic.Enabled = false;            
            this.menuRemoveGraphic.Size = new System.Drawing.Size(67, 22);
            this.menuRemoveGraphic.Click += new System.EventHandler(this.removeGraphic);             
                         
            // MENU REMOVE DATA
             
            this.menuRemoveData.Enabled = false;            
            this.menuRemoveData.Size = new System.Drawing.Size(67, 22);
            this.menuRemoveData.Click += new System.EventHandler(this.removeData);                   
            
            // MENU CLEAR TEXT            
            
            this.menuClearText.Size = new System.Drawing.Size(67, 22);
            this.menuClearText.Click += new System.EventHandler(this.removeText);
            
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////// 
            // MENU ACTION
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////// 
            
            this.actionMenuStripOne.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuCoverText,
            new System.Windows.Forms.ToolStripSeparator(),
            this.menuCoverFile,
            new System.Windows.Forms.ToolStripSeparator(),
            this.menuDiscoverText,
            new System.Windows.Forms.ToolStripSeparator(),
            this.menuUncoverFile});            
            this.actionMenuStripOne.Size = new System.Drawing.Size(12, 20);
             
            // MENU COVER TEXT
             
            this.menuCoverText.Enabled = false;            
            this.menuCoverText.Size = new System.Drawing.Size(67, 22);
            this.menuCoverText.Click += new System.EventHandler(this.StartCoverText);        
             
            // MENU COVER DATA FROM FILE
             
            this.menuCoverFile.Enabled = false;            
            this.menuCoverFile.Size = new System.Drawing.Size(67, 22);
            this.menuCoverFile.Click += new System.EventHandler(this.StartCoverFile);           
             
            // MENU UNCOVER TEXT
             
            this.menuDiscoverText.Enabled = false;           
            this.menuDiscoverText.Size = new System.Drawing.Size(67, 22);
            this.menuDiscoverText.Click += new System.EventHandler(this.StartUncoverText);           
             
            // MENU UNCOVER DATA
             
            this.menuUncoverFile.Enabled = false;           
            this.menuUncoverFile.Size = new System.Drawing.Size(67, 22);
            this.menuUncoverFile.Click += new System.EventHandler(this.StartUncoverFile);
            
            ////////////////////////////////////////////////////////////////////////////////////////////////////// 
            // MENU SETINGS
            ////////////////////////////////////////////////////////////////////////////////////////////////////// 
            
            this.settingsMenuStripOne.Size = new System.Drawing.Size(12, 20);
            this.settingsMenuStripOne.Click += new System.EventHandler(this.showSetting);   
                      
            ////////////////////////////////////////////////////////////////////////////////////////////////////// 
            // MENU INFO
            ////////////////////////////////////////////////////////////////////////////////////////////////////// 
                        
            this.infoMenuStrip.Size = new System.Drawing.Size(12, 20);
            this.infoMenuStrip.Click += new System.EventHandler( this.ShowAbout );                  
                                             
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// 
            // PICTURE BOX
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// 
            
            this.pictureBox.BackColor = System.Drawing.Color.Lavender;
            this.pictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox.Location = new System.Drawing.Point(40, 64);            
            this.pictureBox.Size = new System.Drawing.Size(573, 444);
            this.pictureBox.TabStop = false;            

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// 
            // TEXT CONTROL 
            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////// 
            
            this.textControl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textControl.Location = new System.Drawing.Point(642, 64);
            this.textControl.Multiline = true;           
            this.textControl.Size = new System.Drawing.Size(353, 361);           
            this.textControl.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;                     
            
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////// 
            // DATA FROM FILE CONTROL
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////// 
            
            this.fileNameControl.BackColor = System.Drawing.Color.White;
            this.fileNameControl.Location = new System.Drawing.Point(7, 12);            
            this.fileNameControl.Size = new System.Drawing.Size(334, 24);
                      
            
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////// 
            // PANEL
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////// 
            
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.fileNameControl);
            this.panel1.Location = new System.Drawing.Point(642, 463);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(353, 45);
            this.panel1.TabIndex = 5;
            
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////// 
            // MAIN FORM
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////// 
            
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1018, 574);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.textControl);
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.menuStripOne);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStripOne;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.menuStripOne.ResumeLayout(false);
            this.menuStripOne.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStripOne;
        private System.Windows.Forms.ToolStripMenuItem fileMenuStripOne;
        private System.Windows.Forms.ToolStripMenuItem actionMenuStripOne;
        private System.Windows.Forms.ToolStripMenuItem settingsMenuStripOne;
        private System.Windows.Forms.ToolStripMenuItem infoMenuStrip;
        
        private System.Windows.Forms.ToolStripMenuItem menuOpenGraphic;
        private System.Windows.Forms.ToolStripMenuItem menuOpenFile;
        private System.Windows.Forms.ToolStripMenuItem menuSaveGraphic;
        private System.Windows.Forms.ToolStripMenuItem menuSaveData;
        private System.Windows.Forms.ToolStripMenuItem menuRemoveGraphic;
        private System.Windows.Forms.ToolStripMenuItem menuRemoveData;
        private System.Windows.Forms.ToolStripMenuItem menuClearText;        
              
        private System.Windows.Forms.ToolStripMenuItem menuCoverText;        
        private System.Windows.Forms.ToolStripMenuItem menuDiscoverText;
        private System.Windows.Forms.ToolStripMenuItem menuCoverFile;
        private System.Windows.Forms.ToolStripMenuItem menuUncoverFile;

        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.TextBox textControl;      
                
        private System.Windows.Forms.Label fileNameControl;
        private System.Windows.Forms.Panel panel1;
                
    }
}

