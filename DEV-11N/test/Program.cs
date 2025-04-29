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


// 🧠 Lista en memoria para almacenar usuarios
var users = new List<User>();
var counter=1;

// 🚀 POST: Agregar un usuario
app.MapPost("/users", (User user) =>
{
    user.Id=counter;
    users.Add(user);

    
    counter++;
    return Results.Ok($"Usuario {user.Name} agregado exitosamente");
});

// 🚀 POST: Agregar usuario usando parámetros
app.MapPost("/usersparam", (string name, int age) =>
{
    var user = new User { Name = name, Age = age };
    users.Add(user);
    return Results.Ok($"Usuario {name} agregado exitosamente");
});

// 📄 GET: Buscar usuario por ID en la ruta
app.MapGet("/user/{id}", (int id) =>
{
    var user = users.FirstOrDefault(u => u.Id == id);
    return user is not null ? Results.Ok(user) : Results.NotFound("Usuario no encontrado");
});


// 📄 GET: Obtener todos los usuarios
app.MapGet("/users", () =>
{
    return Results.Ok(users);
});


app.Run();

//public record UserDto(string Name, int Age);

public class User
{
    public int Id { get; set; }
    public int Age { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
}

public class superUser : User
{
    public bool Power { get; set; }
}


