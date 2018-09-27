﻿namespace AbstractBarView
{
    partial class FormMain
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.справочникиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.клиентыToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ингредиентыToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.коктейлиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.складToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.барменыToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.пополнитьСкладToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.buttonPayRequest = new System.Windows.Forms.Button();
            this.buttonRequestReady = new System.Windows.Forms.Button();
            this.buttonTakeRequestInWork = new System.Windows.Forms.Button();
            this.buttonCreateRequest = new System.Windows.Forms.Button();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.buttonRef = new System.Windows.Forms.Button();
            this.отчетыToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.прайсИзделийToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.загруженностьСкладовToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.заказыКлиентовToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.справочникиToolStripMenuItem,
            this.пополнитьСкладToolStripMenuItem,
            this.отчетыToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1028, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // справочникиToolStripMenuItem
            // 
            this.справочникиToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.клиентыToolStripMenuItem,
            this.ингредиентыToolStripMenuItem,
            this.коктейлиToolStripMenuItem,
            this.складToolStripMenuItem,
            this.барменыToolStripMenuItem});
            this.справочникиToolStripMenuItem.Name = "справочникиToolStripMenuItem";
            this.справочникиToolStripMenuItem.Size = new System.Drawing.Size(94, 20);
            this.справочникиToolStripMenuItem.Text = "Справочники";
            // 
            // клиентыToolStripMenuItem
            // 
            this.клиентыToolStripMenuItem.Name = "клиентыToolStripMenuItem";
            this.клиентыToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.клиентыToolStripMenuItem.Text = "Клиенты";
            this.клиентыToolStripMenuItem.Click += new System.EventHandler(this.клиентыToolStripMenuItem_Click);
            // 
            // ингредиентыToolStripMenuItem
            // 
            this.ингредиентыToolStripMenuItem.Name = "ингредиентыToolStripMenuItem";
            this.ингредиентыToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.ингредиентыToolStripMenuItem.Text = "Ингредиенты";
            this.ингредиентыToolStripMenuItem.Click += new System.EventHandler(this.ингредиентыToolStripMenuItem_Click);
            // 
            // коктейлиToolStripMenuItem
            // 
            this.коктейлиToolStripMenuItem.Name = "коктейлиToolStripMenuItem";
            this.коктейлиToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.коктейлиToolStripMenuItem.Text = "Коктейли";
            this.коктейлиToolStripMenuItem.Click += new System.EventHandler(this.коктейлиToolStripMenuItem_Click);
            // 
            // складToolStripMenuItem
            // 
            this.складToolStripMenuItem.Name = "складToolStripMenuItem";
            this.складToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.складToolStripMenuItem.Text = "Хранилище";
            this.складToolStripMenuItem.Click += new System.EventHandler(this.складыToolStripMenuItem_Click);
            // 
            // барменыToolStripMenuItem
            // 
            this.барменыToolStripMenuItem.Name = "барменыToolStripMenuItem";
            this.барменыToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.барменыToolStripMenuItem.Text = "Бармены";
            this.барменыToolStripMenuItem.Click += new System.EventHandler(this.барменыToolStripMenuItem_Click);
            // 
            // пополнитьСкладToolStripMenuItem
            // 
            this.пополнитьСкладToolStripMenuItem.Name = "пополнитьСкладToolStripMenuItem";
            this.пополнитьСкладToolStripMenuItem.Size = new System.Drawing.Size(147, 20);
            this.пополнитьСкладToolStripMenuItem.Text = "Пополнить хранилище";
            this.пополнитьСкладToolStripMenuItem.Click += new System.EventHandler(this.пополнитьСкладToolStripMenuItem_Click);
            // 
            // buttonPayRequest
            // 
            this.buttonPayRequest.Location = new System.Drawing.Point(888, 200);
            this.buttonPayRequest.Name = "buttonPayRequest";
            this.buttonPayRequest.Size = new System.Drawing.Size(149, 23);
            this.buttonPayRequest.TabIndex = 4;
            this.buttonPayRequest.Text = "Заказ оплачен";
            this.buttonPayRequest.UseVisualStyleBackColor = true;
            this.buttonPayRequest.Click += new System.EventHandler(this.buttonPayRequest_Click);
            // 
            // buttonRequestReady
            // 
            this.buttonRequestReady.Location = new System.Drawing.Point(888, 148);
            this.buttonRequestReady.Name = "buttonRequestReady";
            this.buttonRequestReady.Size = new System.Drawing.Size(149, 23);
            this.buttonRequestReady.TabIndex = 3;
            this.buttonRequestReady.Text = "Заказ готов";
            this.buttonRequestReady.UseVisualStyleBackColor = true;
            this.buttonRequestReady.Click += new System.EventHandler(this.buttonRequestReady_Click);
            // 
            // buttonTakeRequestInWork
            // 
            this.buttonTakeRequestInWork.Location = new System.Drawing.Point(888, 101);
            this.buttonTakeRequestInWork.Name = "buttonTakeRequestInWork";
            this.buttonTakeRequestInWork.Size = new System.Drawing.Size(149, 23);
            this.buttonTakeRequestInWork.TabIndex = 2;
            this.buttonTakeRequestInWork.Text = "Отдать на выполнение";
            this.buttonTakeRequestInWork.UseVisualStyleBackColor = true;
            this.buttonTakeRequestInWork.Click += new System.EventHandler(this.buttonTakeRequestInWork_Click);
            // 
            // buttonCreateRequest
            // 
            this.buttonCreateRequest.Location = new System.Drawing.Point(888, 50);
            this.buttonCreateRequest.Name = "buttonCreateRequest";
            this.buttonCreateRequest.Size = new System.Drawing.Size(149, 23);
            this.buttonCreateRequest.TabIndex = 1;
            this.buttonCreateRequest.Text = "Создать заказ";
            this.buttonCreateRequest.UseVisualStyleBackColor = true;
            this.buttonCreateRequest.Click += new System.EventHandler(this.buttonCreateRequest_Click);
            // 
            // dataGridView
            // 
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToDeleteRows = false;
            this.dataGridView.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Dock = System.Windows.Forms.DockStyle.Left;
            this.dataGridView.Location = new System.Drawing.Point(0, 24);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.ReadOnly = true;
            this.dataGridView.Size = new System.Drawing.Size(873, 277);
            this.dataGridView.TabIndex = 0;
            // 
            // buttonRef
            // 
            this.buttonRef.Location = new System.Drawing.Point(888, 251);
            this.buttonRef.Name = "buttonRef";
            this.buttonRef.Size = new System.Drawing.Size(149, 23);
            this.buttonRef.TabIndex = 5;
            this.buttonRef.Text = "Обновить список";
            this.buttonRef.UseVisualStyleBackColor = true;
            this.buttonRef.Click += new System.EventHandler(this.buttonRef_Click);
            // 
            // отчетыToolStripMenuItem
            // 
            this.отчетыToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.прайсИзделийToolStripMenuItem,
            this.загруженностьСкладовToolStripMenuItem,
            this.заказыКлиентовToolStripMenuItem});
            this.отчетыToolStripMenuItem.Name = "отчетыToolStripMenuItem";
            this.отчетыToolStripMenuItem.Size = new System.Drawing.Size(60, 20);
            this.отчетыToolStripMenuItem.Text = "Отчеты";
            // 
            // прайсИзделийToolStripMenuItem
            // 
            this.прайсИзделийToolStripMenuItem.Name = "прайсИзделийToolStripMenuItem";
            this.прайсИзделийToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
            this.прайсИзделийToolStripMenuItem.Text = "Прайс изделий";
            this.прайсИзделийToolStripMenuItem.Click += new System.EventHandler(this.прайсИзделийToolStripMenuItem_Click);
            // 
            // загруженностьСкладовToolStripMenuItem
            // 
            this.загруженностьСкладовToolStripMenuItem.Name = "загруженностьСкладовToolStripMenuItem";
            this.загруженностьСкладовToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
            this.загруженностьСкладовToolStripMenuItem.Text = "Загруженность складов";
            this.загруженностьСкладовToolStripMenuItem.Click += new System.EventHandler(this.загруженностьСкладовToolStripMenuItem_Click);
            // 
            // заказыКлиентовToolStripMenuItem
            // 
            this.заказыКлиентовToolStripMenuItem.Name = "заказыКлиентовToolStripMenuItem";
            this.заказыКлиентовToolStripMenuItem.Size = new System.Drawing.Size(205, 22);
            this.заказыКлиентовToolStripMenuItem.Text = "Заказы клиентов";
            this.заказыКлиентовToolStripMenuItem.Click += new System.EventHandler(this.заказыКлиентовToolStripMenuItem_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1028, 301);
            this.Controls.Add(this.buttonRef);
            this.Controls.Add(this.buttonPayRequest);
            this.Controls.Add(this.buttonRequestReady);
            this.Controls.Add(this.buttonTakeRequestInWork);
            this.Controls.Add(this.buttonCreateRequest);
            this.Controls.Add(this.dataGridView);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FormMain";
            this.Text = "Бар";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem справочникиToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ингредиентыToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem коктейлиToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem складToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem барменыToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem пополнитьСкладToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem клиентыToolStripMenuItem;
        private System.Windows.Forms.Button buttonPayRequest;
        private System.Windows.Forms.Button buttonRequestReady;
        private System.Windows.Forms.Button buttonTakeRequestInWork;
        private System.Windows.Forms.Button buttonCreateRequest;
        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.Button buttonRef;
        private System.Windows.Forms.ToolStripMenuItem отчетыToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem прайсИзделийToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem загруженностьСкладовToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem заказыКлиентовToolStripMenuItem;
    }
}
