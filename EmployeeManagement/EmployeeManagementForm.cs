using EmployeeManagement.Library;
using EmployeeManagement.Properties;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EmployeeManagement
{
    public partial class EmployeeManagement : Form
    {
        private static readonly string url = ConfigurationManager.AppSettings["apiUrl"];

        public EmployeeManagement()
        {
            InitializeComponent();
        }

        #region Private Methods

        private void EmployeeManagement_Load(object sender, EventArgs e)
        {
            GetEmployeeList();
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;
        }
        private void GetEmployeeList()
        {
            string jsonString = "";
            
            jsonString = HttpClient.Create(HttpMethod.Get,url);

            GetEmployeesResponse employeesResponse = JsonConvert.DeserializeObject<GetEmployeesResponse>(jsonString);
            if (employeesResponse.Code == ((int)HttpStatusCode.OK).ToString())
                employeelst.DataSource = employeesResponse.Data;
            else
                MessageBox.Show(Resources.NoRecordsFound);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (txtempid.Text != string.Empty)
            {
                string jsonString = "";
                jsonString = HttpClient.Create(HttpMethod.Get,url + "/" + Convert.ToInt32(txtempid.Text));

                GetEmployeeResponse employee = JsonConvert.DeserializeObject<GetEmployeeResponse>(jsonString);
                if (employee.Code == ((int)HttpStatusCode.OK).ToString())
                {
                    txtEmpName.Text = employee.Data.Name;
                    txtEmail.Text = employee.Data.Email;
                    txtEmpGender.Text = employee.Data.Gender;
                    lblStatus.Text = employee.Data.Status;
                    btnUpdate.Enabled = true;
                    btnDelete.Enabled = true;
                }
                else
                    MessageBox.Show(Resources.NoRecordsFound);
            }
            else
            {
                MessageBox.Show(Resources.InvalidId, Resources.InvalidId, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        #endregion Private Methods

        private void btnsave_Click(object sender, EventArgs e)
        {
            string responseFromServer = "";
            var employee = new Employee()
            {
                Email = txtEmail.Text,
                Gender = txtEmpGender.Text,
                Name = txtEmpName.Text
            };

            responseFromServer = HttpClient.Create(HttpMethod.Post, url, JsonConvert.SerializeObject(employee));

            GetEmployeeResponse employeeResponse = JsonConvert.DeserializeObject<GetEmployeeResponse>(responseFromServer);

            if (employeeResponse.Code == ((int)HttpStatusCode.OK).ToString())
                MessageBox.Show(Resources.EmployeeCreated);
            else
                MessageBox.Show(Resources.UnableToCreate);

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (txtEmail.Text != string.Empty && txtempid.Text != string.Empty && txtEmpName.Text != string.Empty && txtEmpGender.Text != string.Empty)
            {
                string responseFromServer = "";

                var employee = new Employee()
                {
                    Email = txtEmail.Text,
                    Gender = txtEmpGender.Text,
                    Name = txtEmpName.Text
                };

                responseFromServer = HttpClient.Create(HttpMethod.Put, url + "/" + Convert.ToInt32(txtempid.Text), JsonConvert.SerializeObject(employee));

                GetEmployeeResponse employeeResponse = JsonConvert.DeserializeObject<GetEmployeeResponse>(responseFromServer);

                if (employeeResponse.Code == ((int)HttpStatusCode.OK).ToString())
                {
                    MessageBox.Show(Resources.RecordUpdated, Resources.RecordUpdated, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    GetEmployeeList();
                    btnDelete.Enabled = false;
                    btnUpdate.Enabled = false;
                }
                else
                    MessageBox.Show(Resources.UnableToUpdate);
            }
            else
                MessageBox.Show(Resources.EnterFieldValue, Resources.InvalidData, MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (txtempid.Text != string.Empty)
            {
                string jsonString = "";
                jsonString = HttpClient.Create(HttpMethod.Delete,url + "/" + Convert.ToInt32(txtempid.Text));

                GetEmployeeResponse employee = JsonConvert.DeserializeObject<GetEmployeeResponse>(jsonString);
                if (employee.Code == ((int)HttpStatusCode.OK).ToString())
                {
                    MessageBox.Show(Resources.RecordDeleted);
                }
                else
                    MessageBox.Show(Resources.UnableToDelete);
            }
            else
                MessageBox.Show(Resources.EnterValidId, Resources.InvalidId, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (employeelst.Rows.Count > 0)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "CSV (*.csv)|*.csv";
                sfd.FileName = "Exported.csv";
                bool fileError = false;
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    if (File.Exists(sfd.FileName))
                    {
                        try
                        {
                            File.Delete(sfd.FileName);
                        }
                        catch (IOException ex)
                        {
                            fileError = true;
                            MessageBox.Show(ex.Message);
                        }
                    }
                    if (!fileError)
                    {
                        ExpotCSV(employeelst, sfd);
                    }
                }
            }
            else
                MessageBox.Show(Resources.NoRecordsFound, Resources.Info);
        }


        private void ExpotCSV(DataGridView employeelst, SaveFileDialog sfd)
        {
            try
            {
                int columnCount = employeelst.Columns.Count;
                string columnNames = "";
                string[] outputCsv = new string[employeelst.Rows.Count + 1];
                for (int i = 0; i < columnCount; i++)
                {
                    columnNames += employeelst.Columns[i].HeaderText.ToString() + ",";
                }
                outputCsv[0] += columnNames;

                for (int i = 1; (i - 1) < employeelst.Rows.Count; i++)
                {
                    for (int j = 0; j < columnCount; j++)
                    {
                        outputCsv[i] += employeelst.Rows[i - 1].Cells[j].Value.ToString() + ",";
                    }
                }

                File.WriteAllLines(sfd.FileName, outputCsv, Encoding.UTF8);
                MessageBox.Show(Resources.DataExported, Resources.Info);
            }
            catch (Exception ex)
            {
                MessageBox.Show(Resources.Error + ex.Message);
            }
        }
    }
}