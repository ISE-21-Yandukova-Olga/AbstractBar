using AbstractBarService.BindingModel;
using AbstractBarService.Interface;
using AbstractBarService.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Unity;
using Unity.Attributes;

namespace AbstracBarView
{
    public partial class FormCoctail1 : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        public int Id { set { id = value; } }

        private readonly ICoctailService service;

        private int? id;

        private List<CoctailIngredientsViewModel> packageForms;

        public FormCoctail1(ICoctailService service)
        {
            InitializeComponent();
            this.service = service;
        }

        private void FormDocumentPackage_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    CoctailViewModel view = service.GetElement(id.Value);
                    if (view != null)
                    {
                        textBoxName.Text = view.CoctailName;
                        textBoxPrice.Text = view.Price.ToString();
                        packageForms = view.CoctailIngredients;
                        LoadData();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                packageForms = new List<CoctailIngredientsViewModel>();
            }
        }

        private void LoadData()
        {
            try
            {
                if (packageForms != null)
                {
                    dataGridView.DataSource = null;
                    dataGridView.DataSource = packageForms;
                    dataGridView.Columns[0].Visible = false;
                    dataGridView.Columns[1].Visible = false;
                    dataGridView.Columns[2].Visible = false;
                    dataGridView.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormCoctailIngredients>();
            if (form.ShowDialog() == DialogResult.OK)
            {
                if (form.Model != null)
                {
                    if (id.HasValue)
                    {
                        form.Model.Coctail_Id = id.Value;
                    }
                    packageForms.Add(form.Model);
                }
                LoadData();
            }
        }

        private void buttonUpd_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 1)
            {
                var form = Container.Resolve<FormCoctailIngredients>();
                form.Model = packageForms[dataGridView.SelectedRows[0].Cells[0].RowIndex];
                if (form.ShowDialog() == DialogResult.OK)
                {
                    packageForms[dataGridView.SelectedRows[0].Cells[0].RowIndex] = form.Model;
                    LoadData();
                }
            }
        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 1)
            {
                if (MessageBox.Show("Удалить запись", "Вопрос", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        packageForms.RemoveAt(dataGridView.SelectedRows[0].Cells[0].RowIndex);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    LoadData();
                }
            }
        }

        private void buttonRef_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxName.Text))
            {
                MessageBox.Show("Заполните название", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrEmpty(textBoxPrice.Text))
            {
                MessageBox.Show("Заполните цену", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (packageForms == null || packageForms.Count == 0)
            {
                MessageBox.Show("Заполните компоненты", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                List<CoctailIngredientsBindingModel> packageFormBM = new List<CoctailIngredientsBindingModel> ();
                for (int i = 0; i < packageForms.Count; ++i)
                {
                    packageFormBM.Add(new CoctailIngredientsBindingModel
                    {
                        Id = packageForms[i].Id,
                        Coctail_Id = packageForms[i].Coctail_Id,
                        Ingredients_Id = packageForms[i].Ingredients_Id,
                        Count = packageForms[i].Count
                    });
                }
                if (id.HasValue)
                {
                    service.UpdElement(new CoctailBindingModel
                    {
                        Id = id.Value,
                        CoctailName = textBoxName.Text,
                        Price = Convert.ToInt32(textBoxPrice.Text),
                        CoctailIngredients = packageFormBM
                    });
                }
                else
                {
                    service.AddElement(new CoctailBindingModel
                    {
                        CoctailName = textBoxName.Text,
                        Price = Convert.ToInt32(textBoxPrice.Text),
                        CoctailIngredients = packageFormBM
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
