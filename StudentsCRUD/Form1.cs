using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Data.Linq;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StudentsCRUD
{
    
    public partial class Form1 : Form
    {
        students_table model = new students_table();
        List<Panel> listPanel = new List<Panel>();

        Form1 obj = (Form1)Application.OpenForms["FormA"];
        int index;
        public Form1()
        {
            InitializeComponent();

            DataGridViewButtonColumn btn = new DataGridViewButtonColumn();
            btn.HeaderText = "Edit";
            btn.Name = "btnEdit";
            btn.Text = "Edit";
            btn.Width = 50;
            btn.UseColumnTextForButtonValue = true;
            dataGridView1.Columns.Add(btn);
            DataGridViewButtonColumn btn2 = new DataGridViewButtonColumn();
            btn2.HeaderText = "Delete";
            btn2.Name = "btnDelete";
            btn2.Text = "Delete";
            btn2.Width = 50;
            btn2.UseColumnTextForButtonValue = true;
            dataGridView1.Columns.Add(btn2);
            dataGridView1.AllowUserToAddRows = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
            this.students_tableTableAdapter.Fill(this.studentsDataSet.students_table);
            listPanel.Add(panel3);
            listPanel.Add(panel4);
            listPanel[index].BringToFront();
        }
        
     
        
        public void UpdateDG()
        {
            // update datagrid
            StudentsEntities dbe = new StudentsEntities();
            BindingSource bs = new BindingSource();
            dataGridView1.DataSource = null;
            List<students_table> students1 = dbe.students_table.ToList();
            bs.DataSource = students1;
            dataGridView1.DataSource = bs;
            btnAddStudent.Enabled = true;
        }
        private void listOfStudents_Click(object sender, EventArgs e)
        {
            UpdateDG();
            // sad
            if (index > 0)
                {
                    listPanel[--index].BringToFront();
                }
            
        }

        private void addNewStudent_Click(object sender, EventArgs e)
        {
            nameText.Text = "";
            lastnameText.Text = "";
            emailText.Text = "";
            phoneText.Text = "";
            btnEdit.Enabled = false;
            btnEdit.Visible = false;
            btnAdd.Visible = true;
            btnAdd.Enabled = true;
            if (index < listPanel.Count - 1)
            {
                listPanel[++index].BringToFront();
            }
        }

        int valueIndex = 0;
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 5)
            {
                
                listPanel[++index].BringToFront();
                nameText.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                lastnameText.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
                emailText.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
                phoneText.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
                btnAddStudent.Enabled = false;
                btnAdd.Enabled = false;
                btnAdd.Visible = false; 
                btnEdit.Enabled = true;
                btnEdit.Visible = true;
                valueIndex = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
            }
            if (e.ColumnIndex == 6)
            {


                using (var context = new StudentsEntities())
                {
                    
                    int value = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
                    var entity = context.students_table.First(s => s.id.Equals(value));
                    context.students_table.Remove(entity);

                    context.SaveChanges();

                }
                MessageBox.Show("Student deleted");
                StudentsEntities dbe = new StudentsEntities();
                BindingSource bs = new BindingSource();
                dataGridView1.DataSource = null;
                List<students_table> students1 = dbe.students_table.ToList();
                bs.DataSource = students1;
                dataGridView1.DataSource = bs;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            model.name = nameText.Text.Trim();
            model.lastname = lastnameText.Text.Trim();
            model.email = emailText.Text.Trim();
            model.phone = phoneText.Text.Trim();

            using (StudentsEntities db = new StudentsEntities())
            {
                db.students_table.Add(model);
                db.SaveChanges();
            }
            MessageBox.Show("Student added!");
            nameText.Text = "";
            lastnameText.Text = "";
            emailText.Text = "";
            phoneText.Text = "";
            if (index > 0)
            {
                listPanel[--index].BringToFront();
            }
            UpdateDG();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            btnAdd.Enabled = true;
            using (var context = new StudentsEntities())
            {
                var entity = context.students_table.FirstOrDefault(item => item.id == valueIndex);

                
                if (entity != null)
                {
                    entity.name = nameText.Text.Trim(); 
                    entity.lastname = lastnameText.Text.Trim();
                    entity.email = emailText.Text.Trim();
                    entity.phone = phoneText.Text.Trim();
                    context.SaveChanges();
                }
            }
            MessageBox.Show("Student Updated!");
            nameText.Text = "";
            lastnameText.Text = "";
            emailText.Text = "";
            phoneText.Text = "";
            UpdateDG();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StudentsEntities context = new StudentsEntities();
            dataGridView1.DataSource = (from model in context.students_table
                                        where model.name == searchName.Text
                                        select model).ToList();
        }
    }
}
