CREATE TABLE dbo.Appointments
(
    AppointmentId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    PatientId INT NOT NULL,
    DoctorId INT NOT NULL,
    AppointmentDate DATETIME2 NOT NULL,
    Status NVARCHAR(30) NOT NULL CONSTRAINT DF_Appointments_Status DEFAULT ('Scheduled')
);

CREATE INDEX IX_Appointments_PatientId ON dbo.Appointments(PatientId);
CREATE INDEX IX_Appointments_DoctorId_AppointmentDate ON dbo.Appointments(DoctorId, AppointmentDate);
