using JogoGourmet.GameLogic;
using System.Reflection.PortableExecutable;

public class GameEngine
{
    private List<FoodItem> _foodsMassa;
    private List<FoodItem> _foodsOthers;
    const string LASANHA = "lasanha";
    const string BOLO_CHOCOLATE = "Bolo de Chocolate";

    public GameEngine()
    {
        _foodsMassa = new List<FoodItem>();
        _foodsOthers = new List<FoodItem>();
        InitializeFoods();
    }

    private void InitializeFoods()
    {
        _foodsMassa.Add(new FoodItem(LASANHA, []));
    }

    public void StartGame()
    {
        var isMassa = TryConfirmCharacteristic("massa");
        StartGuessing(isMassa ? _foodsMassa : _foodsOthers);
    }

    public void StartGuessing(List<FoodItem> foods)
    {
        var currentFoods = new List<FoodItem>(foods);
        var confirmedCharacteristics = new List<string>();

        if (currentFoods.Any(x => x.Name == LASANHA) && currentFoods.Count == 1 && MakeGuess(currentFoods[0]))
            return;

        var distinctCharacteristics = foods.SelectMany(f => f.Characteristics).Distinct();
        foreach (var characteristic in distinctCharacteristics)
        {
            if (TryConfirmCharacteristic(characteristic))
            {
                confirmedCharacteristics.Add(characteristic);
                currentFoods = currentFoods.Where(f => f.Characteristics.Contains(characteristic)).ToList();
                if (currentFoods.Count == 1)
                {
                    var guessCorrect = MakeGuess(currentFoods[0]);
                    if (guessCorrect) return;
                    break;
                }
            }
            else
            {
                currentFoods = currentFoods.Where(f => !f.Characteristics.Contains(characteristic)).ToList();
                if (currentFoods.Count == 1)
                {
                    var guessCorrect = MakeGuess(currentFoods[0]);
                    if (guessCorrect) return;
                    break;
                }
            }
        }

        if (confirmedCharacteristics.Count == 0 && !currentFoods.Any(x => x.Name == LASANHA))
        {
            var isBoloChocolate = TryConfirmCharacteristic("Bolo de chocolate");
            if (isBoloChocolate)
                return;
        }

        CreateNewFood(currentFoods, confirmedCharacteristics, foods);
    }


    private static bool TryConfirmCharacteristic(string characteristic)
    {
        return MessageBox.Show($"O prato que você pensou é {characteristic}?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes;
    }

    private static bool MakeGuess(FoodItem food)
    {
        if (MessageBox.Show($"O prato que você pensou é {food.Name}?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
        {
            MessageBox.Show("Acertei de novo!", "Jogo Gourmet");
            return true;
        }

        return false;
    }

    private void CreateNewFood(List<FoodItem> currentFoods, List<string> confirmedCharacteristics, List<FoodItem> foods)
    {
        var newFoodName = PromptForInput("Não sei o que você pensou. Qual  prato você pensou?", "Desisto");
        if (newFoodName == null)
        {
            ResetGame();
            return;
        }
        var anotherFoodInList = currentFoods.Count > 0 ? currentFoods.Last().Name : BOLO_CHOCOLATE;
        var newFoodCharacteristic = PromptForInput($"Um(a) {newFoodName} é _____ mas um(a) {anotherFoodInList} não.", "Complete");
        if (newFoodCharacteristic == null)
        {
            ResetGame();
            return;
        }

        confirmedCharacteristics.Add(newFoodCharacteristic);
        foods.Add(new FoodItem(newFoodName, confirmedCharacteristics));
        MessageBox.Show("Nova comida e característica adicionada. Reiniciando o jogo", "Jogo Gourmet");
        StartGame();
    }

    private static string? PromptForInput(string prompt, string titulo)
    {
        Form promptForm = new()
        {
            Width = 500,
            Height = 150,
            FormBorderStyle = FormBorderStyle.FixedDialog,
            Text = titulo,
            StartPosition = FormStartPosition.CenterParent,
        };

        Label textLabel = new() { Left = 50, Top = 20, Width = 450, Height = 50, Text = prompt };
        TextBox textBox = new() { Left = 50, Top = 50, Width = 400 };
        Button confirmationButton = new() { Text = "Ok", Left = 250, Width = 100, Top = 80, DialogResult = DialogResult.OK };
        Button cancelButton = new() { Text = "Cancel", Left = 350, Width = 100, Top = 80, DialogResult = DialogResult.Cancel };
        confirmationButton.Click += (sender, e) => { promptForm.Close(); };
        cancelButton.Click += (sender, e) => { promptForm.Close(); };
        promptForm.Controls.Add(textBox);
        promptForm.Controls.Add(confirmationButton);
        promptForm.Controls.Add(cancelButton);
        promptForm.Controls.Add(textLabel);
        promptForm.AcceptButton = confirmationButton;
        promptForm.CancelButton = cancelButton;

        return promptForm.ShowDialog() == DialogResult.OK ? textBox.Text.Trim().ToLower() : null;
    }

    private void ResetGame()
    {
        MessageBox.Show("Rodada cancelada.", "Jogo Gourmet");
        return;
    }
}