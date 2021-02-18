using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RecipeDB
{
    public partial class Form1 : Form
    {
        string connectionString;
        SqlConnection connection;
        
        public Form1()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["RecipeDB.Properties.Settings.RecipeDBConnectionString"].ConnectionString;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            PopulateRecipies();
        }

        private void PopulateRecipies()
        {
            using(connection = new SqlConnection(connectionString))
            using(SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Recipe", connection))
            {
                DataTable recipeTable = new DataTable();
                adapter.Fill(recipeTable);

                listRecipe.DisplayMember = "Name";
                listRecipe.ValueMember = "Id";
                listRecipe.DataSource = recipeTable;
            }    
        }

        private void PopulateIngredients()
        {
            string query = "SELECT Ingredient.Name FROM Ingredient INNER JOIN RecipeIngredient ON Ingredient.Id = RecipeIngredient.IngredientId WHERE RecipeIngredient.RecipeId = @RecipeId";
            using(connection = new SqlConnection(connectionString))
            using(SqlCommand command = new SqlCommand(query, connection))
            using(SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                //get recipe id from the listRecipe listbox
                command.Parameters.AddWithValue("@RecipeId", listRecipe.SelectedValue);
                DataTable ingredientTable = new DataTable();
                adapter.Fill(ingredientTable);

                listIngredient.DisplayMember = "Name";
                listIngredient.ValueMember = "Id";
                listIngredient.DataSource = ingredientTable;
            }
        }

        private void listRecipe_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateIngredients();
        }
    }
}
