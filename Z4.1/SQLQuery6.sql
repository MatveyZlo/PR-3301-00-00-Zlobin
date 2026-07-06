INSERT INTO Categories (Name) VALUES ('Выпечка'), ('Супы'), ('Напитки');

INSERT INTO Authors (Nickname, Email) VALUES 
('GordonRamsay', 'gordon@hellskitchen.com'),
('JamieO', 'jamie@oliver.uk');

INSERT INTO Ingredients (Title, CaloriesPer100g) VALUES 
('Мука пшеничная', 342.00), ('Сахар', 387.00), ('Вода', 0.00), ('Томаты', 18.00);

-- Получаем ID созданных сущностей для связки
DECLARE @AuthorID INT = (SELECT TOP 1 AuthorID FROM Authors WHERE Nickname = 'GordonRamsay');
DECLARE @CatBakery INT = (SELECT TOP 1 CategoryID FROM Categories WHERE Name = 'Выпечка');
DECLARE @CatSoup INT = (SELECT TOP 1 CategoryID FROM Categories WHERE Name = 'Супы');

DECLARE @BreadID UNIQUEIDENTIFIER = NEWID();
DECLARE @SoupID UNIQUEIDENTIFIER = NEWID();

INSERT INTO Recipes (RecipeID, Title, Rating, IsVegetarian, AuthorID, CategoryID) VALUES 
(@BreadID, 'Домашний хлеб', 4.90, 1, @AuthorID, @CatBakery),
(@SoupID, 'Гаспачо', 4.20, 1, @AuthorID, @CatSoup);

-- Наполняем рецепты ингредиентами (N:N)
INSERT INTO RecipeIngredients (RecipeID, IngredientID, AmountGrams) VALUES 
(@BreadID, 1, 500.00), -- Хлеб: 500г муки
(@BreadID, 2, 10.00),  -- Хлеб: 10г сахара
(@BreadID, 3, 300.00), -- Хлеб: 300г воды
(@SoupID, 4, 600.00);  -- Гаспачо: 600г томатов
GO