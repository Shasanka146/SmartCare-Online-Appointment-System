using SmartCare.Repository.ClinicRepository;
using SmartCare.Shared.ClinicData;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartCare.Business.ClinicBusiness
{
    public class ClinicBusiness : IClinicBusiness
    {
        private readonly IClinicRepository _repository;

        public ClinicBusiness(IClinicRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> AddClinic(ClinicDetails clinic)
        {
            if (clinic == null)
                throw new ArgumentNullException(nameof(clinic));

            if (string.IsNullOrWhiteSpace(clinic.ClinicName))
                throw new Exception("Clinic Name is required");

            return await _repository.AddClinic(clinic);
        }

        public async Task<List<ClinicDetails>> GetAllClinics()
        {
            return await _repository.GetAllClinics();
        }

        public async Task<ClinicDetails> GetClinicById(int clinicId)
        {
            if (clinicId <= 0)
                throw new Exception("Invalid Clinic Id");

            return await _repository.GetClinicById(clinicId);
        }

        public async Task<bool> UpdateClinic(ClinicDetails clinic)
        {
            if (clinic == null || clinic.ClinicId <= 0)
                throw new Exception("Invalid Clinic Data");

            return await _repository.UpdateClinic(clinic);
        }

        public async Task<bool> DeleteClinic(int clinicId)
        {
            if (clinicId <= 0)
                throw new Exception("Invalid Clinic Id");

            return await _repository.DeleteClinic(clinicId);
        }
    }
}