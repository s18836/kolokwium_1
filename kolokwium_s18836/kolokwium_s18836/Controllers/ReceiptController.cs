using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using kolokwium_s18836.DTOs;
using kolokwium_s18836.Models;
using kolokwium_s18836.Services;
using Microsoft.AspNetCore.Mvc;

namespace kolokwium_s18836.Controllers
{
    [ApiController]
    [Route("api/prescriptions")]
    public class ReceiptController : ControllerBase
    {

        private readonly IPrescriptionDBservice _dbService;

        public ReceiptController(IPrescriptionDBservice dbService)
        {
            _dbService = dbService;
        }

   

        [HttpGet]
        public IActionResult GetPrescriptions(string nazwisko)
        {
            PrescriptionDBservice service = new PrescriptionDBservice();
            _dbService.clear();

            using (var con = new SqlConnection("Data Source=db-mssql;Initial Catalog=s18836;Integrated Security=True"))
            using (var com = new SqlCommand())
            {
                com.Connection = con;
                con.Open();
                string text;

                if(nazwisko == null)
                {
                    text = "SELECT IdPrescription,DATE,DueDate,PATIENT.LastName AS PATIENTLASTNAME, Doctor.LastName AS DOCTORLASTNAME FROM Prescription INNER JOIN DOCTOR ON Doctor.IdDoctor = Prescription.IdDoctor INNER JOIN Patient ON Patient.IdPatient = Prescription.IdPatient ORDER BY DATE DESC;";
                }
                else
                {
                    text = $"SELECT IdPrescription, DATE, DueDate, PATIENT.LastName AS PATIENTLASTNAME, Doctor.LastName AS DOCTORLASTNAME FROM Prescription INNER JOIN DOCTOR ON Doctor.IdDoctor = Prescription.IdDoctor INNER JOIN Patient ON Patient.IdPatient = Prescription.IdPatient WHERE DOCTOR.LASTNAME = '{nazwisko}'; ";
                }



                com.CommandText = "USE [S18836];" + text;
                  var dr = com.ExecuteReader();

                  while(dr.Read())
                   {
                       var response = new prescriptionsresponse();
                       response.IdPrescription = (int)dr["IdPrescription"];
                       response.Date = (DateTime)dr["Date"];
                       response.DueDate = (DateTime)dr["DueDate"];
                       response.PatientLastName = dr["PATIENTLASTNAME"].ToString();
                       response.DoctorLastName = dr["DOCTORLASTNAME"].ToString();

                       _dbService.add_prescription(response);
                        
                   }



             
                return Ok(_dbService.get_response()); //(com.CommandText);
            }
               
        
        }


        [HttpPost]
        public void AddPrescriptions(NewPrescriptionRequest request)
        {
            using (var con = new SqlConnection("Data Source=db-mssql;Initial Catalog=s18836;Integrated Security=True"))
            using (var com = new SqlCommand())
            {
               
                    com.Connection = con;
                    con.Open();

                    var tran = con.BeginTransaction();
                    com.Transaction = tran;

                try
                {
                    var wpis = new Wpis();
                    wpis.Date = request.Date;
                    wpis.DueDate = request.DueDate;
                    wpis.IdPatient = request.IdPatient;
                    wpis.IdDoctor = request.IdDoctor;

                    com.CommandText = "USE [2019SBD]; INSERT INTO Prescription VALUES(@id, @date , @duedate , @idpat, @iddoc)";
                    com.Parameters.AddWithValue("@id", 1);
                    com.Parameters.AddWithValue("@date", wpis.Date);
                    com.Parameters.AddWithValue("@duedate", wpis.DueDate);
                    com.Parameters.AddWithValue("@idpat", wpis.IdPatient);
                    com.Parameters.AddWithValue("@iddoc", wpis.IdDoctor);

                    com.ExecuteNonQuery();


                }
                catch (SqlException e)
                {
                    tran.Rollback();
                }

                tran.Commit();
              
            }



        }
    }
}