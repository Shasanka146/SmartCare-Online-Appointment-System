using SmartCare.Shared.PatientData;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartCare.Repository.PatientRepository
{
    public class patientRepository : IPatientRepository
    {
        public Task<bool> AddPatient(PatientDetails patient)
        {
            throw new NotImplementedException();
        }

        public Task<List<PatientDetails>> GetAllPatients()
        {
            throw new NotImplementedException();
        }

        public Task<PatientDetails> GetPatientById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdatePatient(PatientDetails patient)
        {
            throw new NotImplementedException();
        }
    }
}
