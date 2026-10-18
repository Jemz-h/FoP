using Microsoft.AspNetCore.Mvc;
using Prog_Fun.Models;
using System.Diagnostics;
using System.Globalization;

namespace Prog_Fun.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

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

            if (result == null)
            {
                var error = (op == "/" || op == "%") && num2 == 0
                    ? "Cannot divide by zero."
                    : "Unknown operator.";
                return Json(new { expression, error });
            }

            return Json(new { expression, answer = Format(result.Value) });
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
