using AbstractBarService.BindingModels;
using AbstractBarService.Interfaces;
using AbstractBarService.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;

namespace AbstractBarView
{
    public partial class FormCoctail : Form
    {
        public int Id { set { id = value; } }

        private int? id;

        private List<CoctailIngredientViewModel> CoctailIngredients;

        public FormCoctail()
        {
            InitializeComponent();
        }

        private void FormCoctail_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    var response = APIClient.GetRequest("api/Coctail/Get/" + id.Value);
                    if (response.Result.IsSuccessStatusCode)
                    {
                        var Coctail = APIClient.GetElement<CoctailViewModel>(response);
                        textBoxName.Text = Coctail.CoctailName;
                        textBoxPrice.Text = Coctail.Price.ToString();
                        CoctailIngredients = Coctail.CoctailIngredients;
                        LoadData();
                    }
                    else
                    {
                        throw new Exception(APIClient.GetError(response));
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                CoctailIngredients = new List<CoctailIngredientViewModel>();
            }
        }

        private void LoadData()
        {
            try
            {
                if (CoctailIngredients != null)
                {
                    dataGridView.DataSource = null;
                    dataGridView.DataSource = CoctailIngredients;
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
            var form = new FormCoctailIngredient();
            if (form.ShowDialog() == DialogResult.OK)
            {
                if (form.Model != null)
                {
                    if (id.HasValue)
                    {
                        form.Model.CoctailId = id.Value;
                    }
                    CoctailIngredients.Add(form.Model);
                }
                LoadData();
            }
        }

        private void buttonUpd_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 1)
            {
                var form = new FormCoctailIngredient();
                form.Model = CoctailIngredients[dataGridView.SelectedRows[0].Cells[0].RowIndex];
                if (form.ShowDialog() == DialogResult.OK)
                {
                    CoctailIngredients[dataGridView.SelectedRows[0].Cells[0].RowIndex] = form.Model;
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
                        CoctailIngredients.RemoveAt(dataGridView.SelectedRows[0].Cells[0].RowIndex);
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
            if (CoctailIngredients == null || CoctailIngredients.Count == 0)
            {
                MessageBox.Show("Заполните компоненты", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                List<CoctailIngredientBindingModel> CoctailIngredientBM = new List<CoctailIngredientBindingModel>();
                for (int i = 0; i < CoctailIngredients.Count; ++i)
                {
                    CoctailIngredientBM.Add(new CoctailIngredientBindingModel
                    {
                        Id = CoctailIngredients[i].Id,
                        CoctailId = CoctailIngredients[i].CoctailId,
                        IngredientId = CoctailIngredients[i].IngredientId,
                        Count = CoctailIngredients[i].Count
                    });
                }
                Task<HttpResponseMessage> response;
                if (id.HasValue)
                {
                    response = APIClient.PostRequest("api/Coctail/UpdElement", new CoctailBindingModel
                    {
                        Id = id.Value,
                        CoctailName = textBoxName.Text,
                        Price = Convert.ToInt32(textBoxPrice.Text),
                        CoctailIngredients = CoctailIngredientBM
                    });
                }
                else
                {
                    response = APIClient.PostRequest("api/Coctail/AddElement", new CoctailBindingModel
                    {
                        CoctailName = textBoxName.Text,
                        Price = Convert.ToInt32(textBoxPrice.Text),
                        CoctailIngredients = CoctailIngredientBM
                    });
                }
                if (response.Result.IsSuccessStatusCode)
                {
                    MessageBox.Show("Сохранение прошло успешно", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    throw new Exception(APIClient.GetError(response));
                }
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