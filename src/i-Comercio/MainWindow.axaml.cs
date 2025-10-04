using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;
using Avalonia.Media;
using System;

namespace i_Comercio
{
    public partial class MainWindow : Window
    {
        private bool _menuExpanded = true;
        private const double MenuExpandedWidth = 280;
        private const double MenuCollapsedWidth = 70;
        private DispatcherTimer? _timeTimer;
        private string _currentActiveMenu = "";

        public MainWindow()
        {
            InitializeComponent();
            InitializeTimer();
        }

        private void InitializeTimer()
        {
            _timeTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _timeTimer.Tick += (s, e) =>
            {
                var timeControl = this.FindControl<TextBlock>("CurrentTime");
                if (timeControl != null)
                {
                    timeControl.Text = DateTime.Now.ToString("HH:mm:ss");
                }
            };
            _timeTimer.Start();
        }

        // Toggle do Menu Principal
        private void MenuToggle_Click(object sender, RoutedEventArgs e)
        {
            _menuExpanded = !_menuExpanded;
            var menuBorder = this.FindControl<Border>("SideMenuBorder");
            var toggleIcon = this.FindControl<Image>("ToggleIcon");
            
            if (menuBorder != null && toggleIcon != null)
            {
                if (_menuExpanded)
                {
                    menuBorder.Width = MenuExpandedWidth;
                    var chevronLeftDrawing = this.FindResource("ChevronLeftIcon") as DrawingGroup;
                    if (chevronLeftDrawing != null)
                        toggleIcon.Source = new DrawingImage(chevronLeftDrawing);
                    ShowMenuTexts(true);
                }
                else
                {
                    menuBorder.Width = MenuCollapsedWidth;
                    var chevronRightDrawing = this.FindResource("ChevronRightIcon") as DrawingGroup;
                    if (chevronRightDrawing != null)
                        toggleIcon.Source = new DrawingImage(chevronRightDrawing);
                    ShowMenuTexts(false);
                    CollapseAllSubMenus();
                }
            }
        }

        private void ShowMenuTexts(bool show)
        {
            var controls = new[]
            {
                "MenuTitle", "ClientesText", "EstoqueText", "VendasText", 
                "FinanceiroText", "ConfigText", "FooterText"
            };

            foreach (var controlName in controls)
            {
                var control = this.FindControl<TextBlock>(controlName);
                if (control != null)
                {
                    control.Opacity = show ? 1.0 : 0.0;
                }
            }

            // Show/hide arrows
            var arrows = new[]
            {
                "ClientesArrow", "EstoqueArrow", "VendasArrow", 
                "FinanceiroArrow", "ConfigArrow"
            };

            foreach (var arrowName in arrows)
            {
                var arrow = this.FindControl<Image>(arrowName);
                if (arrow != null)
                {
                    arrow.Opacity = show ? 1.0 : 0.0;
                }
            }
        }

        // Eventos dos Menus Principais
        private void MenuClientes_Click(object sender, RoutedEventArgs e)
        {
            if (!_menuExpanded) return;
            ToggleSubMenu("Clientes");
        }

        private void MenuEstoque_Click(object sender, RoutedEventArgs e)
        {
            if (!_menuExpanded) return;
            ToggleSubMenu("Estoque");
        }

        private void MenuVendas_Click(object sender, RoutedEventArgs e)
        {
            if (!_menuExpanded) return;
            ToggleSubMenu("Vendas");
        }

        private void MenuFinanceiro_Click(object sender, RoutedEventArgs e)
        {
            if (!_menuExpanded) return;
            ToggleSubMenu("Financeiro");
        }

        private void MenuConfig_Click(object sender, RoutedEventArgs e)
        {
            if (!_menuExpanded) return;
            ToggleSubMenu("Config");
        }

        private void ToggleSubMenu(string menuName)
        {
            // Primeiro, colapsa todos os outros submenus
            if (_currentActiveMenu != menuName)
            {
                CollapseAllSubMenus();
            }

            var subMenu = this.FindControl<StackPanel>($"{menuName}SubMenu");
            var arrow = this.FindControl<Image>($"{menuName}Arrow");
            var button = this.FindControl<Button>($"{menuName}Button");

            if (subMenu != null && arrow != null && button != null)
            {
                if (subMenu.IsVisible)
                {
                    subMenu.IsVisible = false;
                    var chevronRightDrawing = this.FindResource("ChevronRightIcon") as DrawingGroup;
                    if (chevronRightDrawing != null)
                        arrow.Source = new DrawingImage(chevronRightDrawing);
                    button.Classes.Remove("Active");
                    _currentActiveMenu = "";
                }
                else
                {
                    subMenu.IsVisible = true;
                    var chevronDownDrawing = this.FindResource("ChevronDownIcon") as DrawingGroup;
                    if (chevronDownDrawing != null)
                        arrow.Source = new DrawingImage(chevronDownDrawing);
                    button.Classes.Add("Active");
                    _currentActiveMenu = menuName;
                }
            }
        }

        private void CollapseAllSubMenus()
        {
            var menus = new[] { "Clientes", "Estoque", "Vendas", "Financeiro", "Config" };
            
            foreach (var menu in menus)
            {
                var subMenu = this.FindControl<StackPanel>($"{menu}SubMenu");
                var arrow = this.FindControl<Image>($"{menu}Arrow");
                var button = this.FindControl<Button>($"{menu}Button");

                if (subMenu != null && arrow != null && button != null)
                {
                    subMenu.IsVisible = false;
                    var chevronRightDrawing = this.FindResource("ChevronRightIcon") as DrawingGroup;
                    if (chevronRightDrawing != null)
                        arrow.Source = new DrawingImage(chevronRightDrawing);
                    button.Classes.Remove("Active");
                }
            }
            _currentActiveMenu = "";
        }

        // Eventos dos Submenus - Clientes
        private void ListarClientes_Click(object sender, RoutedEventArgs e)
        {
            ShowContent("Listar Clientes", "Gestão de Clientes", "Aqui you pode visualizar, pesquisar e gerenciar todos os clientes cadastrados no sistema.");
            SetActiveSubmenu(sender);
        }

        private void AdicionarCliente_Click(object sender, RoutedEventArgs e)
        {
            ShowContent("Adicionar Cliente", "Novo Cliente", "Formulário para cadastro de novos clientes com todas as informações necessárias.");
            SetActiveSubmenu(sender);
        }

        private void RelatoriosClientes_Click(object sender, RoutedEventArgs e)
        {
            ShowContent("Relatórios de Clientes", "Relatórios Gerais", "Relatórios gerais sobre clientes, estatísticas e informações consolidadas.");
            SetActiveSubmenu(sender);
        }

        private void RelatoriosDetalhadosClientes_Click(object sender, RoutedEventArgs e)
        {
            ShowContent("Relatórios Detalhados de Clientes", "Análise Detalhada", "Relatórios detalhados com análises avançadas sobre o comportamento dos clientes.");
            SetActiveSubmenu(sender);
        }

        // Eventos dos Submenus - Estoque
        private void ListarProdutos_Click(object sender, RoutedEventArgs e)
        {
            ShowContent("Listar Produtos", "Gestão de Produtos", "Visualize e gerencie todos os produtos do seu estoque com informações detalhadas.");
            SetActiveSubmenu(sender);
        }

        private void AdicionarProduto_Click(object sender, RoutedEventArgs e)
        {
            ShowContent("Adicionar Produto", "Novo Produto", "Formulário para cadastro de novos produtos no sistema de estoque.");
            SetActiveSubmenu(sender);
        }

        private void ControleEstoque_Click(object sender, RoutedEventArgs e)
        {
            ShowContent("Controle de Estoque", "Controle de Estoque", "Monitore níveis de estoque, alertas de produtos em falta e movimentações.");
            SetActiveSubmenu(sender);
        }

        private void RelatorioProdutos_Click(object sender, RoutedEventArgs e)
        {
            ShowContent("Relatórios de Produtos", "Relatórios de Produtos", "Relatórios detalhados sobre performance e movimentação de produtos.");
            SetActiveSubmenu(sender);
        }

        // Eventos dos Submenus - Vendas
        private void PDV_Click(object sender, RoutedEventArgs e)
        {
            ShowContent("PDV - Ponto de Venda", "Ponto de Venda", "Sistema completo de ponto de venda para processar transações e vendas.");
            SetActiveSubmenu(sender);
        }

        private void HistoricoVendas_Click(object sender, RoutedEventArgs e)
        {
            ShowContent("Histórico de Vendas", "Histórico de Vendas", "Consulte o histórico completo de todas as vendas realizadas.");
            SetActiveSubmenu(sender);
        }

        private void RelatoriosVendas_Click(object sender, RoutedEventArgs e)
        {
            ShowContent("Relatórios de Vendas", "Relatórios de Vendas", "Relatórios gerais sobre vendas, faturamento e performance comercial.");
            SetActiveSubmenu(sender);
        }

        private void AnaliseVendas_Click(object sender, RoutedEventArgs e)
        {
            ShowContent("Análise de Vendas", "Análise de Performance", "Análises avançadas de vendas com gráficos e métricas detalhadas.");
            SetActiveSubmenu(sender);
        }

        // Eventos dos Submenus - Financeiro
        private void ContasReceber_Click(object sender, RoutedEventArgs e)
        {
            ShowContent("Contas a Receber", "Contas a Receber", "Gerencie todas as contas a receber, pagamentos pendentes e cobranças.");
            SetActiveSubmenu(sender);
        }

        private void ContasPagar_Click(object sender, RoutedEventArgs e)
        {
            ShowContent("Contas a Pagar", "Contas a Pagar", "Controle todas as contas a pagar, fornecedores e obrigações financeiras.");
            SetActiveSubmenu(sender);
        }

        private void FluxoCaixa_Click(object sender, RoutedEventArgs e)
        {
            ShowContent("Fluxo de Caixa", "Fluxo de Caixa", "Monitore o fluxo de caixa da empresa com entradas e saídas detalhadas.");
            SetActiveSubmenu(sender);
        }

        private void RelatoriosFinanceiros_Click(object sender, RoutedEventArgs e)
        {
            ShowContent("Relatórios Financeiros", "Relatórios Financeiros", "Relatórios financeiros completos com análises de rentabilidade e custos.");
            SetActiveSubmenu(sender);
        }

        // Eventos dos Submenus - Configurações
        private void DadosEmpresa_Click(object sender, RoutedEventArgs e)
        {
            ShowContent("Dados da Empresa", "Dados da Empresa", "Configure as informações básicas da sua empresa e estabelecimento.");
            SetActiveSubmenu(sender);
        }

        private void Usuarios_Click(object sender, RoutedEventArgs e)
        {
            ShowContent("Usuários e Permissões", "Usuários e Permissões", "Gerencie usuários do sistema e configure permissões de acesso.");
            SetActiveSubmenu(sender);
        }

        private void Backup_Click(object sender, RoutedEventArgs e)
        {
            ShowContent("Backup e Segurança", "Backup e Segurança", "Configure backups automáticos e políticas de segurança do sistema.");
            SetActiveSubmenu(sender);
        }

        private void Personalizacao_Click(object sender, RoutedEventArgs e)
        {
            ShowContent("Personalização", "Personalização", "Personalize a aparência e configurações do sistema conforme suas preferências.");
            SetActiveSubmenu(sender);
        }

        // Evento do botão fechar
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        // Métodos auxiliares
        private void ShowContent(string title, string header, string description)
        {
            var contentArea = this.FindControl<ContentControl>("ContentArea");
            var pageTitle = this.FindControl<TextBlock>("PageTitle");
            
            if (contentArea != null && pageTitle != null)
            {
                pageTitle.Text = title;
                
                var stackPanel = new StackPanel
                {
                    HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                    VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center
                };
                
                var headerBlock = new TextBlock
                {
                    Text = header,
                    FontSize = 36,
                    FontWeight = Avalonia.Media.FontWeight.Bold,
                    Foreground = Avalonia.Media.Brushes.DarkSlateGray,
                    HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                    Margin = new Avalonia.Thickness(0, 0, 0, 20)
                };

                var descriptionBlock = new TextBlock
                {
                    Text = description,
                    FontSize = 16,
                    Foreground = Avalonia.Media.Brushes.Gray,
                    HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                    TextAlignment = Avalonia.Media.TextAlignment.Center,
                    TextWrapping = Avalonia.Media.TextWrapping.Wrap,
                    MaxWidth = 600
                };

                // Criar um painel horizontal para o ícone de aviso + texto
                var statusPanel = new StackPanel
                {
                    Orientation = Avalonia.Layout.Orientation.Horizontal,
                    HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                    Margin = new Avalonia.Thickness(0, 30, 0, 0)
                };

                var warningImage = new Image
                {
                    Width = 16,
                    Height = 16,
                    Margin = new Avalonia.Thickness(0, 0, 5, 0),
                    VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center
                };

                var warningDrawing = this.FindResource("WarningIcon") as DrawingGroup;
                if (warningDrawing != null)
                    warningImage.Source = new DrawingImage(warningDrawing);

                var statusBlock = new TextBlock
                {
                    Text = "Esta funcionalidade está em desenvolvimento",
                    FontSize = 14,
                    Foreground = Avalonia.Media.Brushes.Orange,
                    VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
                    FontStyle = Avalonia.Media.FontStyle.Italic
                };

                statusPanel.Children.Add(warningImage);
                statusPanel.Children.Add(statusBlock);

                stackPanel.Children.Add(headerBlock);
                stackPanel.Children.Add(descriptionBlock);
                stackPanel.Children.Add(statusPanel);
                
                contentArea.Content = stackPanel;
            }
        }

        private void SetActiveSubmenu(object sender)
        {
            // Remove active class from all submenus
            ClearActiveSubmenus();
            
            // Add active class to clicked button
            if (sender is Button button)
            {
                button.Classes.Add("Active");
            }
        }

        private void ClearActiveSubmenus()
        {
            var menuItems = this.FindControl<StackPanel>("MenuItems");
            if (menuItems != null)
            {
                foreach (var child in menuItems.Children)
                {
                    if (child is StackPanel menuGroup)
                    {
                        foreach (var subChild in menuGroup.Children)
                        {
                            if (subChild is StackPanel subMenu)
                            {
                                foreach (var subMenuItem in subMenu.Children)
                                {
                                    if (subMenuItem is Button subButton)
                                    {
                                        subButton.Classes.Remove("Active");
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            _timeTimer?.Stop();
            base.OnClosed(e);
        }
    }
}