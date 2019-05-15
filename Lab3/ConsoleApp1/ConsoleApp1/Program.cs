using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            String conStr = @"Data Source=DESKTOP-740KALD\SQLEXPRESS; Initial Catalog=ArtGallery; Integrated Security=true";
            SqlConnection dbCon = new SqlConnection(conStr);
            ThreadStarter ts = new ThreadStarter(dbCon);
            Console.Read();
        }
    }

    class ThreadStarter
    {
        private SqlConnection dbCon;

        public ThreadStarter(SqlConnection dbCon)
        {
            this.dbCon = dbCon;
            this.dbCon.Open();
            this.startThreads();
        }

        void startThreads()
        {
            Thread t1 = new Thread(tran1);
            Thread t2 = new Thread(tran2);
            t1.Start();
            t2.Start();
        }

        void tran1()
        {
            Console.WriteLine("Thread1 started!");
            SqlCommand cmd = new SqlCommand("begin tran update Supervisors set SupervisorFname = 'FN transaction 1'" + 
                " where SupervisorID = 3 waitfor delay '00:00:10' update Sections set SectionType = 'Type transaction 1' where SectionID = 4 commit tran", dbCon);
            cmd.ExecuteNonQuery();
            Console.WriteLine("t1 success");
        }

        void tran2()
        {
            int noTries = 2;
            Console.WriteLine("Thread2 started!");
            SqlCommand cmd = new SqlCommand("begin tran waitfor delay '00:00:2' update Sections set SectionType = 'Type transaction 2'" + 
                " where SectionID = 4 waitfor delay '00:00:10' update Supervisors set SupervisorFname = 'FN transaction 1' where SupervisorID = 3 commit tran", dbCon);

            while (noTries>0)
            {
                try
                {
                    cmd.ExecuteNonQuery();
                    Console.WriteLine("t2 success");
                    break;
                }
                catch (SqlException)
                {
                    Console.WriteLine("t2 deadlock");
                    noTries--;
                }
            }

            if (noTries == 0) Console.WriteLine("t2 aborted");
        }
    }
}
