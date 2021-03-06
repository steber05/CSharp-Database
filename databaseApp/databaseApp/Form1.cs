﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace databaseApp
{
    public partial class Form1 : Form
    {
        String connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=D:\\code\\local\\database\\databaseapp\\FarmInfomation.accdb;Persist Security Info=False";
        String errorMsg = "";
        String q1 = "";
        OleDbConnection conn = null;
        bool validEntry = false;


        public Form1()
        {
            InitializeComponent();
        }

        //Default actions when loading the application
        private void Form1_Load(object sender, EventArgs e)
        {
            connectToolStripMenuItem.PerformClick();
        }//End of default loading proccess

        private void connectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                conn = new OleDbConnection(connString);
                conn.Open();
                disconnectToolStripMenuItem.Enabled = true;
                connectToolStripMenuItem.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }//End of connect function

        //Start query
        private void runToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            //Checks for a connection to database
            if(conn.State == ConnectionState.Open)
            {
                runTableQuery();
                if (validEntry && textBox2.Text != "")
                {
                    runIDQuery();
                }
            }
            else
            {
                MessageBox.Show("No connection to database");
            }
            this.Cursor = Cursors.Default;
        }//End of query

        private void runTableQuery()
        {
            string check = textBox1.Text.ToLower();
            errorMsg = "";

            if (textBox1.Text == "")
            {
                MessageBox.Show("Enter a table name");
            }
            else
            {
                //Check user input
                switch (check)
                {
                    case "commodity":
                        q1 = "SELECT * FROM [COMMODITY PRICES]";
                        validEntry = true;
                        break;
                    case "cows":
                        q1 = "SELECT * FROM COWS";
                        validEntry = true;
                        break;
                    case "dogs":
                        q1 = "SELECT * FROM DOGS";
                        validEntry = true;
                        break;
                    case "goats":
                        q1 = "SELECT * FROM GOATS";
                        validEntry = true;
                        break;
                    case "sheep":
                        q1 = "SELECT * FROM SHEEP";
                        validEntry = true;
                        break;
                    default:
                        MessageBox.Show("\tEnter a valid table name\n(Commodity, Cows, Dogs, Goats, Sheep)");
                        break;
                }
            }
            //Run query if there is a valid input
            if (validEntry)
            {
                try
                {
                    OleDbCommand cmd = new OleDbCommand(q1, conn);
                    OleDbDataAdapter a = new OleDbDataAdapter(cmd);
                    DataTable dt = new DataTable();

                    a.SelectCommand = cmd;
                    a.Fill(dt);

                    results.DataSource = dt;
                    results.AutoResizeColumns();
                }
                catch (Exception ex)
                {
                    errorMsg = ex.Message;
                    MessageBox.Show(errorMsg);
                }

            }
        }//End of runTableQuery

        private void runIDQuery()
        {
            //adjust query to suit the table entered in table textbox
            string table = textBox1.Text;
            string q1 = "SELECT * FROM " + table + " WHERE ID = " + textBox2.Text;
            
            if(textBox1.Text == "")
            {
                MessageBox.Show("Please enter a table name first");
            }            
            //Had no proper way to check values between as they adjusted per table
            //so checked if ID textbox is empty
            else if(textBox2.Text == "")
            {
                MessageBox.Show("Enter a ID");
            }
            else if(validEntry)
            {
                try
                {
                    OleDbCommand cmd = new OleDbCommand(q1, conn);
                    OleDbDataAdapter a = new OleDbDataAdapter(cmd);
                    DataTable dt = new DataTable();

                    a.SelectCommand = cmd;
                    a.Fill(dt);

                    results2.DataSource = dt;
                    results2.AutoResizeColumns();
                    //Second count checks if a table is returned 
                    //if no rows return then it must be a invalid ID
                    //display a message to enter a valid ID
                    if (dt.Rows.Count < 1)
                    {
                        MessageBox.Show("Enter a valid ID");
                    }
                }
                catch (Exception ex)
                {
                    errorMsg = ex.Message;
                    MessageBox.Show(errorMsg);
                }
            }   
        }//End of runIDQuery

        //Disconnect button
        private void disconnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //Have error checked and this function does close the connection
                conn.Close();             
                //Added this to disable the connection completely until connection is re-established
                conn.Dispose();
                disconnectToolStripMenuItem.Enabled = false;
                connectToolStripMenuItem.Enabled = true;
                textBox1.Text = "";
                textBox2.Text = "";
                results.DataSource = null;
                results2.DataSource = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }//End of disconnect

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }//End of exit function

        //Default actions when closing the application
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            disconnectToolStripMenuItem.PerformClick();
        }//End of default closing function
    }
}
