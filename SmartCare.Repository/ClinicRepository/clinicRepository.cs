using Microsoft.Data.Sqlite;
using SmartCare.Repository.ClinicRepository;
using SmartCare.Shared.ClinicData;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartCare.Repository.ClinicRepository
{
    public class ClinicRepository : IClinicRepository
    {
        private readonly string _connectionString;

        public ClinicRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<bool> AddClinic(ClinicDetails clinic)
        {
            using var con = new SqliteConnection(_connectionString);

            string query = @"INSERT INTO Clinics 
                            (ClinicName, Address, PhoneNumber, Email)
                            VALUES (@ClinicName, @Address, @PhoneNumber, @Email)";

            using var cmd = new SqliteCommand(query, con);
            cmd.Parameters.AddWithValue("@ClinicName", clinic.ClinicName);
            cmd.Parameters.AddWithValue("@Address", clinic.Address ?? "");
            cmd.Parameters.AddWithValue("@PhoneNumber", clinic.PhoneNumber ?? "");
            cmd.Parameters.AddWithValue("@Email", clinic.Email ?? "");

            await con.OpenAsync();
            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        public async Task<List<ClinicDetails>> GetAllClinics()
        {
            var list = new List<ClinicDetails>();

            using var con = new SqliteConnection(_connectionString);
            string query = "SELECT * FROM Clinics";

            using var cmd = new SqliteCommand(query, con);
            await con.OpenAsync();

            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                list.Add(new ClinicDetails
                {
                    ClinicId = reader.GetInt32(0),
                    ClinicName = reader.GetString(1),
                    Address = reader.IsDBNull(2) ? "" : reader.GetString(2),
                    PhoneNumber = reader.IsDBNull(3) ? "" : reader.GetString(3),
                    Email = reader.IsDBNull(4) ? "" : reader.GetString(4)
                });
            }

            return list;
        }

        public async Task<ClinicDetails> GetClinicById(int clinicId)
        {
            using var con = new SqliteConnection(_connectionString);

            string query = "SELECT * FROM Clinics WHERE ClinicId = @Id";

            using var cmd = new SqliteCommand(query, con);
            cmd.Parameters.AddWithValue("@Id", clinicId);

            await con.OpenAsync();

            using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new ClinicDetails
                {
                    ClinicId = reader.GetInt32(0),
                    ClinicName = reader.GetString(1),
                    Address = reader.IsDBNull(2) ? "" : reader.GetString(2),
                    PhoneNumber = reader.IsDBNull(3) ? "" : reader.GetString(3),
                    Email = reader.IsDBNull(4) ? "" : reader.GetString(4)
                };
            }

            return null;
        }

        public async Task<bool> UpdateClinic(ClinicDetails clinic)
        {
            using var con = new SqliteConnection(_connectionString);

            string query = @"UPDATE Clinics 
                             SET ClinicName=@ClinicName, Address=@Address,
                                 PhoneNumber=@PhoneNumber, Email=@Email
                             WHERE ClinicId=@Id";

            using var cmd = new SqliteCommand(query, con);
            cmd.Parameters.AddWithValue("@ClinicName", clinic.ClinicName);
            cmd.Parameters.AddWithValue("@Address", clinic.Address ?? "");
            cmd.Parameters.AddWithValue("@PhoneNumber", clinic.PhoneNumber ?? "");
            cmd.Parameters.AddWithValue("@Email", clinic.Email ?? "");
            cmd.Parameters.AddWithValue("@Id", clinic.ClinicId);

            await con.OpenAsync();
            return await cmd.ExecuteNonQueryAsync() > 0;
        }

        public async Task<bool> DeleteClinic(int clinicId)
        {
            using var con = new SqliteConnection(_connectionString);

            string query = "DELETE FROM Clinics WHERE ClinicId = @Id";

            using var cmd = new SqliteCommand(query, con);
            cmd.Parameters.AddWithValue("@Id", clinicId);

            await con.OpenAsync();
            return await cmd.ExecuteNonQueryAsync() > 0;
        }
    }
}