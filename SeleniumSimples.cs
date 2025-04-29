// 1 - Namespace ~ Pacote ~ Grupo de Classe ~ Workspace
namespace SeleniumSimples
{
       // 2 - Bibliotecas ~ Dependências
    using OpenQA.Selenium; // movido para ca surgiu no inicio do scrip quando foi incluido Iwebdriver driver na classe
    using OpenQA.Selenium.Chrome;
    using WebDriverManager;
    using WebDriverManager.DriverConfigs.Impl;
    using NUnit.Framework;
    using NUnit.Framework.Legacy; //silvia

    // 3 - Classe
    [TestFixture] // Configura como uma classe de teste
    public class AdicionarProdutoNoCarrinhoTest
    {
        // 3.1 - Atributos ~Características ~ Campos
#pragma warning disable NUnit1032 // An IDisposable field/property should be Disposed in a TearDown method
        private OpenQA.Selenium.IWebDriver driver; // 
#pragma warning restore NUnit1032 // An IDisposable field/property should be Disposed in a TearDown method

        // 3.2 - Função ou Método de Apoio
        public static IEnumerable<TestCaseData> lerDadosDeTeste()
        {
            // declaramos um objeto chamado reader que lê o conteúdo do arquivo login.csv
            using (var reader = new StreamReader(@"C:\Iterasys\Projeto-Selenium-CSharp\login.csv"))
            {
                // Pular a linha do cabeçalho do csv
                reader.ReadLine(); // estou mandando lera linha1 e nao fazer nada com ela

                // Faça enquanto não for o final do arquivo
                // --   while --  ! -----reader.EndOfStream
                while (!reader.EndOfStream)
                {
                    // ler alinha correspondente
                    var linha = reader.ReadLine();
                    var valores = linha.Split(", ");

                    yield return new TestCaseData(valores[0], valores[1], valores[2]);
                    // fim do while - funciona como uma mola - vai repetindo as operaçoes ate chegar o final do arquivo
                }
            };
        }

        // 3.3 - Configurações de Antes do Teste
        [SetUp] // Confitura método para ser executado antes do teste
        public void Before()
        {
            // Instancia ou faz o download e instalação da versão mais recente do ChromeDriver
            new DriverManager().SetUpDriver(new ChromeConfig());
            driver = new ChromeDriver(); // Instancia=liga o objeto do Selenium como Chrome
            driver.Manage().Window.Maximize(); // Maximiza a janela do navegador
            // Configura uma espera de 5 segundos para qualquer elemento aparecer
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(5000);

        } // Fim do Before

        // 3.4 - Configurações de Depois do Teste
        [TearDown] // Configura um método para ser usado depois dos testes
        public void After()
        {
            driver.Quit(); // Destroi o objeto do Selenium na memória
        } // Fim do After

        // 3.5 - O(s) Teste(s)
        [Test] // Indica que é um método de teste
        public void login()
        {
            // abrir o navegador e acessar o site
            driver.Navigate().GoToUrl("https://www.saucedemo.com");

            // preencher o usuario
            driver.FindElement(By.Id("user-name")).SendKeys("standard_user");

            // preencher a senha
            driver.FindElement(By.Name("password")).SendKeys("secret_sauce");
            
            // clicar no botao 
            driver.FindElement(By.CssSelector("input.submit-button.btn_action")).Click();

            // verificar se foi feito login no sistema, confirmando um texto âncora
            //ClassicAssert.AreEqual("Products", driver.FindElement(By.CssSelector("span.title")).Text);
            // Assert.AreEqual(driver.FindElement(By.CssSelector("span.title")).Text, Is.EqualTo("Products"));
            Assert.That(driver.FindElement(By.CssSelector("span.title")).Text, Is.EqualTo("Products"));
            // para montar a cssselector primeirapalavradatag.oqueestánaclass span.title
            // colocado assert na frente depois, já que faltou indicar o que fazer

            Thread.Sleep(2000);
                    
        } // fim do teste login

        [Test, TestCaseSource("lerDadosDeTeste")] // Indica q é teste que lê dados de algum lugar
        public void loginPositivoDDT(String usuario, String senha, String resultadoEsperado)
        {
            // abrir o navegador e acessar o site
            driver.Navigate().GoToUrl("https://www.saucedemo.com");

            // preencher o usuario
            driver.FindElement(By.Id("user-name")).SendKeys(usuario);

            // preencher a senha
            driver.FindElement(By.Name("password")).SendKeys(senha);
            
            // clicar no botao 
            driver.FindElement(By.CssSelector("input.submit-button.btn_action")).Click();

            Assert.That(driver.FindElement(By.CssSelector("span.title")).Text, Is.EqualTo(resultadoEsperado));

            Thread.Sleep(2000); // pausa forçada remover antes de publicar
        } // fim do tdste de login
    } // fim da classe
}