CREATE DATABASE Conta;
USE Conta;

-- Adicionar função de validação do CPF (generica)
CREATE FUNCTION dbo.ValidarCPF (@CPF CHAR(11))
RETURNS BIT
AS
BEGIN
    DECLARE @Soma INT, @Resto INT;
    DECLARE @Digito INT = CAST(SUBSTRING(@CPF, 10, 2) AS INT);
    SET @Soma = 0;

    -- Verificar se o CPF tem 11 dígitos
    IF LEN(@CPF) <> 11 RETURN 0;

    -- Calcular o primeiro dígito verificador
    DECLARE @Contador INT = 10;
    DECLARE @Posicao INT = 1;

    WHILE @Contador >= 2
    BEGIN
        SET @Soma = @Soma + (CAST(SUBSTRING(@CPF, @Posicao, 1) AS INT) * @Contador);
        SET @Contador = @Contador - 1;
        SET @Posicao = @Posicao + 1;
    END

    SET @Resto = @Soma % 11;

    SET @Digito = 0;

    IF @Resto < 2
        SET @Digito = 0;
    ELSE
        SET @Digito = 11 - @Resto;

    IF @Digito <> CAST(SUBSTRING(@CPF, 10, 1) AS INT) RETURN 0;

    -- Calcular o segundo dígito verificador
    SET @Soma = 0;
    SET @Contador = 11;
    SET @Posicao = 1;

    WHILE @Contador >= 2
    BEGIN
        SET @Soma = @Soma + (CAST(SUBSTRING(@CPF, @Posicao, 1) AS INT) * @Contador);
        SET @Contador = @Contador - 1;
        SET @Posicao = @Posicao + 1;
    END

    SET @Resto = @Soma % 11;

    SET @Digito = 0;

    IF @Resto < 2
        SET @Digito = 0;
    ELSE
        SET @Digito = 11 - @Resto;

    IF @Digito <> CAST(SUBSTRING(@CPF, 11, 1) AS INT) RETURN 0;

    RETURN 1; -- CPF válido
END;


-- Criar tabela Cliente com restrição CHECK para validar CPF
CREATE TABLE Cliente (
    CPF CHAR(11) PRIMARY KEY CHECK (dbo.ValidarCPF(CPF) = 1),
    Nome VARCHAR(100) NOT NULL,
    UF CHAR(2) NOT NULL,
    Celular VARCHAR(20) NOT NULL
);

-- Criar tabela Financiamento
CREATE TABLE Financiamento (
    IdFinanciamento INT PRIMARY KEY,
    CPF CHAR(11) FOREIGN KEY REFERENCES Cliente(CPF),
    TipoFinanciamento VARCHAR(50) NOT NULL,
    ValorTotal DECIMAL(10, 2) NOT NULL,
    DataUltimoVencimento DATE NOT NULL
);

-- Criar tabela Parcela
CREATE TABLE Parcela (
    IdParcela INT PRIMARY KEY,
    IdFinanciamento INT FOREIGN KEY REFERENCES Financiamento(IdFinanciamento),
    NumeroParcela INT NOT NULL,
    ValorParcela DECIMAL(10, 2) NOT NULL,
    DataVencimento DATE NOT NULL,
    DataPagamento DATE
);

-- Criando alguns indexes para a melhora de certas consultas (ficticias)
CREATE INDEX idx_UF ON Cliente(UF);
CREATE INDEX idx_DataVencimento ON Parcela(DataVencimento);
CREATE INDEX idx_DataPagamento ON Parcela(DataPagamento);


-- Inserir clientes sem financiamentos
INSERT INTO Cliente (CPF, Nome, UF, Celular) VALUES
('30379041006', 'Lucia', 'SP', '123456789'),
('84283741043', 'Eduardo', 'SP', '987654321'),
('56819801033', 'Carla', 'SP', '555555555'),
('44021297022', 'Mariano', 'SP', '987987987'),
('56564785054', 'Camila', 'SP', '654654654'),
('43913995048', 'Henrique', 'SP', '321321321');


-- Inserir mais clientes com financiamentos e parcelas
INSERT INTO Cliente (CPF, Nome, UF, Celular) VALUES
('98435536084', 'Rodrigo', 'SP', '111111111'),
('57118194026', 'Patricia', 'SP', '222222222'),
('71314356003', 'Luiza', 'SP', '333333333'),
('33300727090', 'Carlos', 'SP', '444444444'),
('33664325001', 'Bianca', 'SP', '555555555'),
('60036511056', 'Gustavo', 'SP', '666666666');

INSERT INTO Financiamento (IdFinanciamento, CPF, TipoFinanciamento, ValorTotal, DataUltimoVencimento) VALUES
(19, '98435536084', 'Financiamento2', 12000.00, '2024-12-31'),
(20, '57118194026', 'Financiamento3', 8000.00, '2024-12-31'),
(21, '71314356003', 'Financiamento2', 10000.00, '2043-12-31'),
(22, '33300727090', 'Financiamento2', 9400.00, '2024-12-31'),
(23, '33664325001', 'Financiamento2', 11000.00, '2043-12-31'),
(24, '60036511056', 'Financiamento2', 8700.00, '2024-12-31');

INSERT INTO Parcela (IdParcela, IdFinanciamento, NumeroParcela, ValorParcela, DataVencimento, DataPagamento) VALUES
(46, 19, 1, 2000.00, '2024-01-15', '2023-01-20'),
(47, 19, 2, 2000.00, '2024-02-15', NULL),
(48, 19, 3, 2000.00, '2024-03-15', NULL),
(49, 20, 1, 1000.00, '2024-01-15', '2023-01-20'),
(50, 20, 2, 1000.00, '2024-02-15', NULL),
(51, 20, 3, 1000.00, '2024-03-15', NULL),
(52, 21, 1, 1500.00, '2024-01-15', '2023-01-20'),
(53, 21, 2, 1500.00, '2024-02-15', NULL),
(54, 21, 3, 1500.00, '2024-03-15', NULL),
(55, 22, 1, 1800.00, '2024-01-15', '2023-01-20'),
(56, 22, 2, 1800.00, '2024-02-15', NULL),
(57, 22, 3, 1800.00, '2024-03-15', NULL),
(58, 23, 1, 1300.00, '2024-01-15', '2023-01-20'),
(59, 23, 2, 1300.00, '2024-02-15', NULL),
(60, 23, 3, 1300.00, '2024-03-15', NULL);

-- Inserir clientes para a consulta 2
INSERT INTO Cliente (CPF, Nome, UF, Celular) VALUES
('59544665056', 'Cliente1', 'SP', '123456789'),
('83499245000', 'Cliente2', 'SP', '987654321'),
('70614968003', 'Cliente3', 'SP', '555555555'),
('42117509096', 'Cliente4', 'SP', '444444444'),
('65966914036', 'Cliente5', 'SP', '666666666'),
('26704147020', 'Cliente6', 'SP', '777777777');

-- Inserir financiamentos para a consulta 2
INSERT INTO Financiamento (IdFinanciamento, CPF, TipoFinanciamento, ValorTotal, DataUltimoVencimento) VALUES
(1, '59544665056', 'Financiamento1', 5000.00, '2023-12-31'),
(2, '83499245000', 'Financiamento2', 7000.00, '2023-12-31'),
(3, '70614968003', 'Financiamento3', 8000.00, '2023-12-31'),
(4, '42117509096', 'Financiamento4', 6000.00, '2023-12-31'),
(5, '65966914036', 'Financiamento5', 9000.00, '2023-12-31'),
(6, '26704147020', 'Financiamento6', 7500.00, '2023-12-31');

-- Inserir parcelas para a consulta 2
INSERT INTO Parcela (IdParcela, IdFinanciamento, NumeroParcela, ValorParcela, DataVencimento, DataPagamento) VALUES
(1, 1, 1, 1000.00, '2023-01-01', NULL),
(2, 1, 2, 1000.00, '2023-02-01', NULL),
(3, 1, 3, 1000.00, '2023-03-01', '2023-03-05'),
(4, 2, 1, 1500.00, '2023-01-10', '2023-01-15'),
(5, 2, 2, 1500.00, '2023-02-10', NULL),
(6, 2, 3, 1500.00, '2023-03-10', NULL),
(7, 3, 1, 1200.00, '2023-01-05', '2023-01-10'),
(8, 3, 2, 1200.00, '2023-02-05', NULL),
(9, 3, 3, 1200.00, '2023-03-05', NULL),
(10, 4, 1, 800.00, '2023-01-15', '2023-01-20'),
(11, 4, 2, 800.00, '2023-02-15', NULL),
(12, 4, 3, 800.00, '2023-03-15', NULL),
(13, 5, 1, 2000.00, '2023-01-20', '2023-01-25'),
(14, 5, 2, 2000.00, '2023-02-20', NULL),
(15, 5, 3, 2000.00, '2023-03-20', '2023-03-25'),
(16, 6, 1, 1800.00, '2023-01-25', NULL),
(17, 6, 2, 1800.00, '2023-02-25', NULL),
(18, 6, 3, 1800.00, '2023-03-25', NULL);


-- Consulta 1: Listar todos os clientes do estado de SP que possuem mais de 60% das parcelas pagas
SELECT c.Nome, c.CPF
FROM Cliente c
INNER JOIN Financiamento f ON c.CPF = f.CPF
INNER JOIN Parcela p ON f.IdFinanciamento = p.IdFinanciamento
WHERE c.UF = 'SP'
GROUP BY c.Nome, c.CPF
HAVING COUNT(p.IdParcela) > 0.6 * COUNT(CASE WHEN p.DataPagamento IS NOT NULL THEN 1 END);

-- Consulta 2: Listar os primeiros quatro clientes que possuem alguma parcela com mais de cinco dias sem atraso
SELECT TOP 4 c.Nome, c.CPF
FROM Cliente c
INNER JOIN Financiamento f ON c.CPF = f.CPF
INNER JOIN Parcela p ON f.IdFinanciamento = p.IdFinanciamento
WHERE DATEDIFF(DAY, p.DataVencimento, GETDATE()) > 5 AND p.DataPagamento IS NULL
GROUP BY c.Nome, c.CPF;

