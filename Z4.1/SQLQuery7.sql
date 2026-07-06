SELECT Title, Rating, CreatedAt
FROM Recipes
WHERE IsVegetarian = 1 AND Rating >= 4.5
ORDER BY Rating DESC;

-- Поднять рейтинг конкретному рецепту
UPDATE Recipes 
SET Rating = 5.00 
WHERE Title = 'Домашний хлеб';

-- Удалить категорию "Напитки" 
DELETE FROM Categories 
WHERE Name = 'Напитки';

-- Посчитать, сколько всего рецептов написал каждый автор
SELECT A.Nickname, COUNT(R.RecipeID) AS TotalRecipesPosted
FROM Authors A
LEFT JOIN Recipes R ON A.AuthorID = R.AuthorID
GROUP BY A.Nickname;

-- 1. INNER JOIN: Вывести названия рецептов и имена поваров, их создавших
SELECT R.Title AS RecipeName, A.Nickname AS Chef
FROM Recipes R
INNER JOIN Authors A ON R.AuthorID = A.AuthorID;

-- 2. LEFT JOIN: Показать ВСЕ категории, и если в них есть рецепты — вывести их (покажет даже пустые категории)
SELECT C.Name AS Category, R.Title AS Recipe
FROM Categories C
LEFT JOIN Recipes R ON C.CategoryID = R.CategoryID;

-- 3. RIGHT JOIN: Вывести полный список ингредиентов, и если они куда-то добавлены — показать ID рецепта
SELECT RI.RecipeID, I.Title AS IngredientUsed
FROM RecipeIngredients RI
RIGHT JOIN Ingredients I ON RI.IngredientID = I.IngredientID;