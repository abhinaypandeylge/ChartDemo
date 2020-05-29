using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.OleDb;
using System.Data;
using System.Web.Services;
using System.Collections;

namespace ChartsDemo
{
    public class value
    {
        public string Project { get; set; }
        public string CountOpen { get; set; }
        public string CountFixed { get; set; }
        public string CountInprogressDueIn { get; set; }
        public string CountInprogressDueout { get; set; }

    }
    public partial class _default : System.Web.UI.Page
    {
        private static OleDbConnection OledbConn;
        private static OleDbCommand OledbCmd;
        [WebMethod]
        public static object LoadData()
        {
            InitializeOledbConnection("E:\\ChartsDemo\\ChartsDemo\\Data\\SampleData2.xls", ".xls");
            DataTable tempTable = ReadFile();
            IList<value> chartItem = new List<value>();

            //var project = "Cayman 4G";
            //int CountFixed = 4, CountOpen = 5, CountInprogressDueIn = 6, CountInprogressDueOut = 3;
            
            foreach (DataRow item in tempTable.Rows)
            {
                value Chart = new value();
                Chart.Project = item[0].ToString();
                Chart.CountOpen = tempTable.AsEnumerable().Where(x => x.Field<string>("Status") == "Open" && x.Field<string>("Project")== Chart.Project).Select(e => e.Field<string>("Count")).FirstOrDefault()==null ? "0": tempTable.AsEnumerable().Where(x => x.Field<string>("Status") == "Open" && x.Field<string>("Project") == Chart.Project).Select(e => e.Field<string>("Count")).FirstOrDefault();
                Chart.CountInprogressDueIn = tempTable.AsEnumerable().Where(x => x.Field<string>("Status") == "Inprogress Due in" && x.Field<string>("Project") == Chart.Project).Select(e => e.Field<string>("Count")).FirstOrDefault() == null ? "0" : tempTable.AsEnumerable().Where(x => x.Field<string>("Status") == "Inprogress Due in" && x.Field<string>("Project") == Chart.Project).Select(e => e.Field<string>("Count")).FirstOrDefault();
                
                Chart.CountInprogressDueout = tempTable.AsEnumerable().Where(x => x.Field<string>("Status") == "Inprogress Due out" && x.Field<string>("Project") == Chart.Project).Select(e => e.Field<string>("Count")).FirstOrDefault() == null ? "0" : tempTable.AsEnumerable().Where(x => x.Field<string>("Status") == "Inprogress Due out" && x.Field<string>("Project") == Chart.Project).Select(e => e.Field<string>("Count")).FirstOrDefault();

                Chart.CountFixed = tempTable.AsEnumerable().Where(x => x.Field<string>("Status") == "Fixed" && x.Field<string>("Project") == Chart.Project).Select(e => e.Field<string>("Count")).FirstOrDefault() == null ? "0" : tempTable.AsEnumerable().Where(x => x.Field<string>("Status") == "Fixed" && x.Field<string>("Project") == Chart.Project).Select(e => e.Field<string>("Count")).FirstOrDefault();
                if (!(chartItem.Select(e=>e.Project).Contains(Chart.Project)))
                {
                    chartItem.Add(Chart);
                }
            }

            return chartItem.Distinct();
          
        }
        protected void Page_Load(object sender, EventArgs e)
        {
                      
        }

         static void InitializeOledbConnection(string filename, string extrn)
        {
            string connString = "";

            if (extrn == ".xls")
            //Connectionstring for excel v8.0    
            {
                connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source='"+ filename + "';Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=1\"";
            }
            else
            {
                //Connectionstring fo excel v12.0    
                connString = "Provider=Microsoft.Jet.OLEDB.12.0;Data Source=\""+ filename + "\";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=1\"";
            }

            OledbConn = new OleDbConnection(connString);
        }

        private static DataTable ReadFile()
        {
            try
            {

                DataTable schemaTable = new DataTable();
                OledbCmd = new OleDbCommand();
                OledbCmd.Connection = OledbConn;
                OledbConn.Open();
                OledbCmd.CommandText = "Select * from [Output$]";
                OleDbDataReader dr = OledbCmd.ExecuteReader();
                DataTable ContentTable = null;
                if (dr.HasRows)
                {
                    ContentTable = new DataTable();
                    ContentTable.Columns.Add("Project", typeof(string));
                    ContentTable.Columns.Add("Status", typeof(string));
                    ContentTable.Columns.Add("Count", typeof(string));
                    //ContentTable.Columns.Add("Project", typeof(string));

                    while (dr.Read())
                    {
                        if (dr[0].ToString().Trim() != string.Empty && dr[1].ToString().Trim() != string.Empty && dr[2].ToString().Trim() != string.Empty && dr[0].ToString().Trim() != " " && dr[1].ToString().Trim() != " " && dr[2].ToString().Trim() != " ")
                            ContentTable.Rows.Add(dr[0].ToString().Trim(), dr[1].ToString().Trim(), dr[2].ToString().Trim());

                    }
                }
                dr.Close();
                OledbConn.Close();
                return ContentTable;
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }
    }
}