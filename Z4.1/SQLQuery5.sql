CREATE DATABASE CookingHub;
GO

USE CookingHub;
GO

-- 1. Таблица "Категории"
CREATE TABLE Categories (
    CategoryID INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(50) NOT NULL
);

-- 2. Таблица "Авторы"
CREATE TABLE Authors (
    AuthorID INT PRIMARY KEY IDENTITY(1,1),
    Nickname NVARCHAR(100) NOT NULL,
    Email NVARCHAR(150) NOT NULL,          
    RegisteredAt DATETIME2 DEFAULT GETDATE() 
);

-- 3. Таблица "Рецепты" (Связи 1:N с Категориями и Авторами)
CREATE TABLE Recipes (
    RecipeID UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(), 
    Title NVARCHAR(200) NOT NULL,                         
    Rating DECIMAL(3, 2) DEFAULT 0.00,                     
    IsVegetarian BIT DEFAULT 0,                            
    CreatedAt DATETIME2 DEFAULT GETDATE(),                 
    AuthorID INT NOT NULL,
    CategoryID INT NOT NULL,
    CONSTRAINT FK_Recipes_Authors FOREIGN KEY (AuthorID) REFERENCES Authors(AuthorID),
    CONSTRAINT FK_Recipes_Categories FOREIGN KEY (CategoryID) REFERENCES Categories(CategoryID)
);

-- 4. Таблица "Ингредиенты"
CREATE TABLE Ingredients (
    IngredientID INT PRIMARY KEY IDENTITY(1,1),
    Title NVARCHAR(100) NOT NULL,
    CaloriesPer100g DECIMAL(6, 2)                          
);

-- 5. Таблица связи N:N (Состав рецепта)
CREATE TABLE RecipeIngredients (
    RecipeID UNIQUEIDENTIFIER NOT NULL,
    IngredientID INT NOT NULL,
    AmountGrams DECIMAL(7, 2) NOT NULL,                    
    PRIMARY KEY (RecipeID, IngredientID),                 
    CONSTRAINT FK_RI_Recipes FOREIGN KEY (RecipeID) REFERENCES Recipes(RecipeID) ON DELETE CASCADE,
    CONSTRAINT FK_RI_Ingredients FOREIGN KEY (IngredientID) REFERENCES Ingredients(IngredientID)
);
GO