using AbstractBarService.BindingModels;
using AbstractBarService.Interfaces;
using AbstractBarService.ViewModels;
using System;
using System.Collections.Generic;

using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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
                    var Coctail = Task.Run(() => APIClient.GetRequestData<CoctailViewModel>("api/Coctail/Get/" + id.Value)).Result;
                    textBoxName.Text = Coctail.CoctailName;
                    textBoxPrice.Text = Coctail.Price.ToString();
                    CoctailIngredients = Coctail.CoctailIngredients;
                    LoadData();
                }
                catch (Exception ex)
                {
                    while (ex.InnerException != null)
                    {
                        ex = ex.InnerException;
                    }
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
            string name = textBoxName.Text;
            int price = Convert.ToInt32(textBoxPrice.Text);
            Task task;
            if (id.HasValue)
            {
                task = Task.Run(() => APIClient.PostRequestData("api/Coctail/UpdElement", new CoctailBindingModel
                {
                    Id = id.Value,
                    CoctailName = name,
                    Price = price,
                    CoctailIngredients = CoctailIngredientBM
                }));
            }
            else
            {
                task = Task.Run(() => APIClient.PostRequestData("api/Coctail/AddElement", new CoctailBindingModel
                {
                    CoctailName = name,
                    Price = price,
                    CoctailIngredients = CoctailIngredientBM
                }));
            }

            task.ContinueWith((prevTask) => MessageBox.Show("Сохранение прошло успешно. Обновите список", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information),
                TaskContinuationOptions.OnlyOnRanToCompletion);
            task.ContinueWith((prevTask) =>
            {
                var ex = (Exception)prevTask.Exception;
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }, TaskContinuationOptions.OnlyOnFaulted);

            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}