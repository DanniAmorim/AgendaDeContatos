using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace AgendaTelefonica
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection("server=127.0.0.1;userid=root;password=root;database=Agenda"))
                {
                    conn.Open();

                    string query = "INSERT INTO Contatos (Nome,Telefone) VALUES (@Nome, @Telefone)";

                    using (MySqlCommand comando = new MySqlCommand(query, conn))
                    {
                        comando.Parameters.AddWithValue("@Nome", txtNome.Text);
                        comando.Parameters.AddWithValue("@Telefone", txtTelefone.Text);

                        int resultado = comando.ExecuteNonQuery();

                        if (resultado > 0)
                        {
                            MessageBox.Show("Contato salvo com sucesso!");
                        }
                        else
                        {
                            MessageBox.Show("Erro ao salvar o Contato.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao conectar ao banco de dados: {ex.Message}");
            }
            finally
            {
                txtNome.Text = null;
                txtTelefone.Text = null;
            }
        }

        private void btnVisualizar_Click(object sender, EventArgs e)
        {
            string connectionString = "server=127.0.0.1;userid=root;password=root;database=Agenda";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "SELECT Id, Nome, Telefone FROM Contatos";
                MySqlCommand command = new MySqlCommand(query, connection);

                try
                {
                    connection.Open();
                    MySqlDataReader reader = command.ExecuteReader();

                    lstContatos.Items.Clear();
                    while (reader.Read())
                    {
                        string contato = $"({reader["Id"]}) {reader["Nome"]} - {reader["Telefone"]}";
                        lstContatos.Items.Add(contato);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro: " + ex.Message);
                }
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            {
                string connectionString = "server=127.0.0.1;userid=root;password=root;database=Agenda";
                string nomeBuscado = txtBuscar.Text;  

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    
                    string query = "SELECT Nome, Telefone FROM Contatos WHERE Nome LIKE @Nome";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Nome", "%" + nomeBuscado + "%");

                    try
                    {
                        connection.Open();
                        MySqlDataReader reader = command.ExecuteReader();

                        lstContatos.Items.Clear();  

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                string contato = $"{reader["Nome"]} - {reader["Telefone"]}";
                                lstContatos.Items.Add(contato);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Nenhum contato encontrado com o nome especificado.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erro: " + ex.Message);
                    }
                    finally
                    {
                        txtBuscar.Text = null;
                    }
                }
            }
        }
    }
}