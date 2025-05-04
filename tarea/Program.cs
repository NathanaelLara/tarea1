using ApiHospital.Models;
using Microsoft.VisualBasic;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var hospitals = new List<Hospital>();
var doctors = new List<Doctor>();
var doctorCounter = 1;
var hospitalCounter = 1;

app.MapPost("/hospitals", (Hospital hospital) =>
{
    hospital.Id = hospitalCounter++;
    hospitals.Add(hospital);
    return Results.Created($"/hospitals/{hospital.Id}", hospital);
});

app.MapGet("/hospitals", () => Results.Ok(hospitals));

app.MapGet("/hospitals/{id}", (int id) =>
{
    var hospital = hospitals.FirstOrDefault(h => h.Id == id);
    return hospital is not null ? Results.Ok(hospital) : Results.NotFound();
});

app.MapPut("/hospitals/{id}", (int id, Hospital updateHospital) =>
{
    var hospital = hospitals.FirstOrDefault(h => h.Id == id);
    if (hospital is null) return Results.NotFound();

    hospital.Name = updateHospital.Name;
    hospital.Address = updateHospital.Address;
    return Results.Ok(hospital);

});

app.MapDelete("/hospitals/{id}", (int id) =>
{
    var hospital = hospitals.FirstOrDefault(h => h.Id == id);
    if (hospital is null) return Results.NotFound();

    hospitals.Remove(hospital);
    return Results.NoContent();
});


app.MapPost("/doctors", (Doctor doctor) => 
{
    if (!hospitals.Any(h => h.Id == doctor.HospitalId))
        return Results.BadRequest("Hospital Not Found");

    doctor.Id = doctorCounter++;
    doctors.Add(doctor);
    return Results.Created($"/doctors/{doctor.Id}", doctor);
});

app.MapGet("/doctors", () => Results.Ok(doctors));

app.MapGet("/doctors/{id}", (int id) => 
{

    var doctor = doctors.FirstOrDefault(d => d.Id == id);
    return doctor is not null ? Results.Ok(doctor) : Results.NotFound();
});

app.MapPut("/doctors/{id}", (int id, Doctor updatedDoctor) =>
{    
    var doctor = doctors.FirstOrDefault(d => d.Id == id);
    if (doctor is null) return Results.NotFound();

    if(!hospitals.Any(h => h.Id == updatedDoctor.HospitalId))
        return Results.BadRequest("Hospital not found");

        doctor.Name = updatedDoctor.Name;
        doctor.Specialty = updatedDoctor.Specialty;
        doctor.HospitalId = updatedDoctor.HospitalId;
        return Results.Ok(doctor);
});

app.MapDelete("/doctors/{id}", (int id) =>
{
    var doctor = doctors.FirstOrDefault(d => d.Id == id);
    if (doctor is null) return Results.NotFound();

    doctors.Remove(doctor);
    return Results.NoContent();
}); 
