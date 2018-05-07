
using AbstractBarService.BindingModel;
using AbstractBarService.Interface;
using System;
using System.Windows.Forms;
using Unity;
using Unity.Attributes;

namespace AbstractBarView
{
    public partial class Barmen : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        public int Id { set { id = value; } }

        private readonly IBarmenService service;

        private int? id;

        public Barmen (IBarmenService service)
        {
            InitializeComponent();
            this.service = service;
        }

        private void FormBarmen_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    BarmenViewModel view = service.GetElement(id.Value);
                    if (view != null)
                    {
                        textBoxFIO.Text = view.BarmenFIO;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxFIO.Text))
            {
                MessageBox.Show("Заполните ФИО", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                if (id.HasValue)
                {
                    service.UpdElement(new BarmenBindingModel
                    {
                        Id = id.Value,
                        BarmenFIO = textBoxFIO.Text
                    });
                }
                else
                {
                    service.AddElement(new BarmenBindingModel
                    {
                        BarmenFIO = textBoxFIO.Text
                    });
                }
                MessageBox.Show("Сохранение прошло успешно", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
