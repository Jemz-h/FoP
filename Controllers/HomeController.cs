using Microsoft.AspNetCore.Mvc;
using Prog_Fun.Models;
using System.Diagnostics;
using System.Globalization;

namespace Prog_Fun.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Privacy()
        {
            return View();
        }

        // ── Get Input From User ──
        public IActionResult GetInput()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GetInput(string userInput)
        {
            ViewBag.UserInput = userInput;
            return View();
        }

        // ── Sum of Two Numbers ──
        public IActionResult SumOfTwoNumbers()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SumOfTwoNumbers(double num1, double num2)
        {
            ViewBag.Num1 = num1;
            ViewBag.Num2 = num2;
            ViewBag.Sum  = num1 + num2;
            return View();
        }

        // ── Basic Computation (calculator) ──
        public IActionResult BasicComputation()
        {
            return View();
        }

        // Called by the calculator keypad with fetch(); replies with JSON, not a page
        [HttpPost]
        public IActionResult BasicComputation(double num1, string op, double num2)
        {
            double? result = op switch
            {
                "+"                => num1 + num2,
                "-"                => num1 - num2,
                "*"                => num1 * num2,
                "/" when num2 != 0 => num1 / num2,
                "%" when num2 != 0 => num1 % num2,
                _                  => null
            };

            var symbol = op switch { "*" => "×", "/" => "÷", "-" => "−", _ => op };
            var expression = $"{Format(num1)} {symbol} {Format(num2)}";

            ViewBag.Num1 = num1;
            ViewBag.Num2 = num2;
            ViewBag.Op = op;
            ViewBag.Expression = expression;

            if (result == null)
            {
                var error = (op == "/" || op == "%") && num2 == 0
                    ? "Cannot divide by zero."
                    : "Unknown operator.";
                ViewBag.Error = error;

                if (Request.Headers.Accept.ToString().Contains("application/json"))
                {
                    return Json(new { expression, error });
                }
                return View();
            }

            ViewBag.Answer = Format(result.Value);

            if (Request.Headers.Accept.ToString().Contains("application/json"))
            {
                return Json(new { expression, answer = Format(result.Value) });
            }

            return View();
        }

        // Rounds away tiny floating-point noise (0.1 + 0.2)
        private static string Format(double value) =>
            Math.Round(value, 10).ToString(CultureInfo.InvariantCulture);

        // ── Populating Fields ──
        [HttpGet]
        public IActionResult PopulatingFields(bool sample = false)
        {
            if (sample)
            {
                ViewBag.Name = "Maria Santos";
                ViewBag.Age  = 20;
                ViewBag.City = "Quezon City";
            }
            return View();
        }

        [HttpPost]
        public IActionResult PopulatingFields(string name, int age, string city)
        {
            ViewBag.Name      = name;
            ViewBag.Age       = age;
            ViewBag.City      = city;
            ViewBag.Submitted = true;
            return View();
        }

        // ── Conditional Statement (Student Grade Evaluator) ──
        [HttpGet]
        public IActionResult ConditionalStatement()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ConditionalStatement(double? score, double? presetScore = null)
        {
            score = presetScore ?? score;

            string branch;
            string grade;
            string message;
            string color;   // Bootstrap color name used by the alert container

            if (score == null || score < 0 || score > 100)
            {
                branch  = "if (score == null || score < 0 || score > 100)";
                grade   = "Invalid Score";
                message = "Score must be a valid number between 0 and 100.";
                color   = "danger";
            }
            else if (score >= 90)
            {
                branch  = "else if (score >= 90)";
                grade   = "Grade A (Excellent)";
                message = "Outstanding performance! High Honors standing.";
                color   = "success";
            }
            else if (score >= 80)
            {
                branch  = "else if (score >= 80)";
                grade   = "Grade B (Very Good)";
                message = "Very good mastery! Dean's List candidate.";
                color   = "primary";
            }
            else if (score >= 75)
            {
                branch  = "else if (score >= 75)";
                grade   = "Grade C (Satisfactory)";
                message = "Passed standard curriculum benchmarks.";
                color   = "info";
            }
            else if (score >= 60)
            {
                branch  = "else if (score >= 60)";
                grade   = "Grade D (Conditional / Remedial)";
                message = "Marginal pass. Supplementary review recommended.";
                color   = "warning";
            }
            else
            {
                branch  = "else";
                grade   = "Grade F (Failing)";
                message = "Did not meet passing criteria. Course retake required.";
                color   = "danger";
            }

            ViewBag.Score   = score;
            ViewBag.Branch  = branch;
            ViewBag.Grade   = grade;
            ViewBag.Message = message;
            ViewBag.Color   = color;
            return View();
        }

        // ── Array ──
        // The array is sent back and forth with each request (hidden fields), so the
        // controller rebuilds it every time. Arrays have a fixed size, so adding or
        // removing a value creates a new array.
        [HttpGet]
        public IActionResult Array()
        {
            ViewBag.Items = new string[] { "Chocolate", "Mocha", "Latte" };
            return View();
        }

        [HttpPost]
        public IActionResult Array(string[] items, string command, string addValue, int? removeIndex, int? printIndex)
        {
            items ??= new string[0];

            if (command == "add")
            {
                if (string.IsNullOrWhiteSpace(addValue))
                {
                    ViewBag.Message = "Please type a value to add.";
                    ViewBag.Color   = "warning";
                }
                else
                {
                    items = items.Append(addValue.Trim()).ToArray();
                    ViewBag.Message = $"Added \"{addValue.Trim()}\" at index {items.Length - 1}.";
                    ViewBag.Color   = "success";
                }
            }
            else if (command == "remove")
            {
                if (removeIndex == null || removeIndex < 0 || removeIndex >= items.Length)
                {
                    ViewBag.Message = items.Length == 0
                        ? "The array is empty. Nothing to remove."
                        : $"Enter an index from 0 to {items.Length - 1}.";
                    ViewBag.Color = "warning";
                }
                else
                {
                    var removed = items[removeIndex.Value];
                    items = items.Where((_, i) => i != removeIndex).ToArray();
                    ViewBag.Message = $"Removed \"{removed}\" from index {removeIndex}.";
                    ViewBag.Color   = "danger";
                }
            }
            else if (command == "print")
            {
                if (printIndex == null || printIndex < 0 || printIndex >= items.Length)
                {
                    ViewBag.Message = items.Length == 0
                        ? "The array is empty. Nothing to print."
                        : $"Enter an index from 0 to {items.Length - 1}.";
                    ViewBag.Color = "warning";
                }
                else
                {
                    ViewBag.Message = $"items[{printIndex}] = \"{items[printIndex.Value]}\"";
                    ViewBag.Color   = "info";
                }
            }

            ViewBag.Items = items;
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
