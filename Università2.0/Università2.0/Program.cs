using Università2._0.Classi;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});


builder.Services.AddEndpointsApiExplorer();


builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Access", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Access",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Inserisci il token di accesso (es. BRTDF129012)"
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Access"
                }
            },
            new List<string>()
        }
    });
});



var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseCors("AllowAll");


app.Use(async (context, next) =>
{
    if (context.Request.Path == "/login")
        await next.Invoke();
    else if (context.Request.Headers.TryGetValue("Access", out var valoreToken) && valoreToken == "BRTDF129012")
        await next.Invoke();
    else
    {
        context.Response.StatusCode = StatusCodes.Status403Forbidden;
        await context.Response.WriteAsync("Accesso negato: token non valido o mancante.");
    }
});


var corsi = new List<Corso>
{
    new() { Id = 1, Nome = "Matematica", IsDisponibile = true },
    new() { Id = 2, Nome = "Fisica", IsDisponibile = true },
    new() { Id = 3, Nome = "Letteratura Italiana", IsDisponibile = true },
    new() { Id = 4, Nome = "Storia Contemporanea", IsDisponibile = false },
    new() { Id = 5, Nome = "Informatica", IsDisponibile = true },
    new() { Id = 6, Nome = "Economia Politica", IsDisponibile = true },
    new() { Id = 7, Nome = "Psicologia Generale", IsDisponibile = false },
    new() { Id = 8, Nome = "Biologia", IsDisponibile = true },
    new() { Id = 9, Nome = "Diritto Costituzionale", IsDisponibile = true },
    new() { Id = 10, Nome = "Sociologia", IsDisponibile = true },
    new() { Id = 11, Nome = "Educazione Civica", IsDisponibile = true },
    new() { Id = 12, Nome = "Geografia", IsDisponibile = false }
};


var studenti = new List<Studente>
{
    new() { Id = 11, Nome = "Matteo", Cognome = "Russo", Matricola = "A011", IsIscritto = true, Corsi = corsi.Where(c => c.Nome == "Educazione Civica" || c.Nome == "Geografia").ToList() },
    new() { Id = 12, Nome = "Giulia", Cognome = "Bianchi", Matricola = "A012", IsIscritto = true, Corsi = corsi.Where(c => c.Nome == "Matematica" || c.Nome == "Fisica").ToList() },
    new() { Id = 13, Nome = "Luca", Cognome = "Verdi", Matricola = "A013", IsIscritto = false, Corsi = corsi.Where(c => c.Nome == "Psicologia Generale" || c.Nome == "Sociologia").ToList() },
    new() { Id = 14, Nome = "Sara", Cognome = "Romano", Matricola = "A014", IsIscritto = true, Corsi = corsi.Where(c => c.Nome == "Economia Politica" || c.Nome == "Diritto Costituzionale").ToList() },
    new() { Id = 15, Nome = "Alessandro", Cognome = "Ferrari", Matricola = "A015", IsIscritto = true, Corsi = corsi.Where(c => c.Nome == "Informatica" || c.Nome == "Biologia").ToList() },
    new() { Id = 16, Nome = "Chiara", Cognome = "Gallo", Matricola = "A016", IsIscritto = false, Corsi = corsi.Where(c => c.Nome == "Letteratura Italiana" || c.Nome == "Storia Contemporanea").ToList() },
    new() { Id = 17, Nome = "Davide", Cognome = "Conti", Matricola = "A017", IsIscritto = true, Corsi = corsi.Where(c => c.Nome == "Matematica" || c.Nome == "Informatica").ToList() },
    new() { Id = 18, Nome = "Elisa", Cognome = "Marini", Matricola = "A018", IsIscritto = true, Corsi = corsi.Where(c => c.Nome == "Fisica" || c.Nome == "Economia Politica").ToList() },
    new() { Id = 19, Nome = "Francesco", Cognome = "Greco", Matricola = "A019", IsIscritto = false, Corsi = corsi.Where(c => c.Nome == "Educazione Civica" || c.Nome == "Diritto Costituzionale").ToList() },
    new() { Id = 20, Nome = "Martina", Cognome = "Rinaldi", Matricola = "A020", IsIscritto = true, Corsi = corsi.Where(c => c.Nome == "Sociologia" || c.Nome == "Biologia").ToList() }
};


app.MapPost("/login", (Credenziali cred) =>
{
    if (string.IsNullOrWhiteSpace(cred.Username) || string.IsNullOrWhiteSpace(cred.Password))
        return Results.BadRequest();

    if (cred.Username == "Dante" && cred.Password == "Alighieri")
        return Results.Ok(new { token = "BRTDF129012" });

    return Results.BadRequest();
});

app.MapGet("/profilo", () => "info sensibili utente");


app.MapGet("/api/studenti", () => Results.Ok(studenti));

app.MapPost("/api/studenti", (Studente stu) =>
{
    if (studenti.Any(s => s.Matricola == stu.Matricola))
        return Results.BadRequest("Matricola già esistente");

    stu.Id = studenti.Max(s => s.Id) + 1;
    studenti.Add(stu);
    return Results.Created($"/api/studenti/{stu.Matricola}", stu);
});

app.MapGet("/api/studenti/{matricola}", (string matricola) =>
{
    var studente = studenti.FirstOrDefault(s => s.Matricola == matricola);
    return studente is not null ? Results.Ok(studente) : Results.NotFound();
});

app.MapGet("/api/studenti/{matricola}/corsi-disponibili", (string matricola) =>
{
    var studente = studenti.FirstOrDefault(s => s.Matricola == matricola);
    if (studente is null) return Results.NotFound();

    var disponibili = studente.Corsi.Where(c => c.IsDisponibile).ToList();
    return Results.Ok(disponibili);
});

app.MapGet("/api/studenti/iscritti", () =>
{
    var iscritti = studenti.Where(s => s.IsIscritto).ToList();
    return Results.Ok(iscritti);
});


app.MapGet("/api/corsi", () => Results.Ok(corsi));

app.MapGet("/api/corsi/{id}", (int id) =>
{
    var corso = corsi.FirstOrDefault(c => c.Id == id);
    return corso is not null ? Results.Ok(corso) : Results.NotFound();
});

app.MapPost("/api/corsi", (Corso corso) =>
{
    corso.Id = corsi.Max(c => c.Id) + 1;
    corsi.Add(corso);
    return Results.Created($"/api/corsi/{corso.Id}", corso);
});

app.MapPut("/api/corsi/{id}", (int id, Corso input) =>
{
    var corso = corsi.FirstOrDefault(c => c.Id == id);
    if (corso is null) return Results.NotFound();

    corso.Nome = input.Nome;
    corso.IsDisponibile = input.IsDisponibile;
    return Results.Ok(corso);
});

app.MapDelete("/api/corsi/{id}", (int id) =>
{
    var corso = corsi.FirstOrDefault(c => c.Id == id);
    if (corso is null) return Results.NotFound();

    corsi.Remove(corso);
    return Results.NoContent();
});

app.MapGet("/api/corsi/disponibili", () =>
{
    var disponibili = corsi.Where(c => c.IsDisponibile).ToList();
    return Results.Ok(disponibili);
});

app.Run();
