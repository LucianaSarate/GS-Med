CREATE TABLE Pacientes (
    idPaciente SERIAL  PRIMARY KEY,
    nombreApellido TEXT,
    fecha_nacimiento DATE,
    telefono TEXT,
    direccion TEXT,
    ocupacion TEXT,
    descripcionOcupacion TEXT,
    peso TEXT,
    medicamentos_que_toma TEXT,
    otras_terapias TEXT,
    operaciones TEXT,
    metales_en_el_cuerpo TEXT,
    otros_problemas TEXT
);

CREATE TABLE Consultas (
    idConsulta SERIAL  PRIMARY KEY,
    idPaciente INT REFERENCES Pacientes(idPaciente),
    fecha DATE,
    motivo TEXT,
    observaciones TEXT
);