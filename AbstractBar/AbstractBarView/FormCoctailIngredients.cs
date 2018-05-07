using AbstractBarService.Interface;
using AbstractBarService.ViewModel;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using Unity;
using Unity.Attributes;

namespace AbstracBarView
{
    public partial class FormCoctailIngredients : Form
    {
        [Unity.Attributes.Dependency]
        public new IUnityContainer Container { get; set; }

        public CoctailIngredientsViewModel Model { set { model = value; } get { return model; } }

        private readonly IIngredientsService service;

        private CoctailIngredientsViewModel model;

        public FormCoctailIngredients(IIngredientsService service)
        {
            InitializeComponent();
            this.service = service;
        }

        private void FormCoctailIngredients_Load(object sender, EventArgs e)
        {
            try
            {
                List<IngredientsViewModel> list = service.GetList();
                if (list != null)
                {
                    comboBoxComponent.DisplayMember = "IngredientsName";
                    comboBoxComponent.ValueMember = "Id";
                    comboBoxComponent.DataSource = list;
                    comboBoxComponent.SelectedItem = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (model != null)
            {
                comboBoxComponent.Enabled = false;
                comboBoxComponent.SelectedValue = model.Ingredients_Id;
                textBoxCount.Text = model.Count.ToString();
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxCount.Text))
            {
                MessageBox.Show("Заполните поле Количество", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (comboBoxComponent.SelectedValue == null)
            {
                MessageBox.Show("Выберите компонент", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                if (model == null)
                {
                    model = new CoctailIngredientsViewModel
                    {
                        Ingredients_Id = Convert.ToInt32(comboBoxComponent.SelectedValue),
                        IngredientsName = comboBoxComponent.Text,
                        Count = Convert.ToInt32(textBoxCount.Text)
                    };
                }
                else
                {
                    model.Count = Convert.ToInt32(textBoxCount.Text);
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
