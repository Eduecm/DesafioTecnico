<%@ Page Language="vb" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Associados.aspx.vb" Inherits="DesafioTecnico.Associados" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        // Formata o CPF enquanto o usuário digita
        function formatarCpf(elemento) {
            var valor = elemento.value.replace(/\D/g, ''); // Remove todos os caracteres não numéricos
            if (valor.length > 3) {
                valor = valor.substring(0, 3) + '.' + valor.substring(3);
            }
            if (valor.length > 7) {
                valor = valor.substring(0, 7) + '.' + valor.substring(7);
            }
            if (valor.length > 11) {
                valor = valor.substring(0, 11) + '-' + valor.substring(11);
            }
            elemento.value = valor;
        }

        // Formata a data enquanto o usuário digita
        function formatarData(elemento) {
            var valor = elemento.value;
            if (valor.length === 2 || valor.length === 5) {
                valor += '/';
                elemento.value = valor;
            }
        }
    </script>
    <main>
        <div class="container">
            <h1>Associados</h1>
            <div class="form-group">
                <label for="txtId" class="label">Id:</label>
                <div class="input-group">
                    <asp:TextBox ID="txtId" runat="server" placeholder="Digite o Id para pesquisar" Text="Novo" CssClass="form-control" Style="margin-right: 10px;"></asp:TextBox>
                    <div class="input-group-append">
                        <asp:Button ID="btnPesquisar" runat="server" Text="Pesquisar" OnClick="btnPesquisar_Click" CssClass="btn btn-primary" />
                    </div>
                </div>
            </div>
            <div class="form-group">
                <label for="txtNome" class="label">Nome:</label>
                <asp:TextBox ID="txtNome" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="form-group">
                <label for="txtCpf" class="label">CPF:</label>
                <asp:TextBox ID="txtCpf" runat="server" CssClass="form-control" onkeypress="formatarCpf(this)"></asp:TextBox>
            </div>
            <div class="form-group">
                <label for="txtDataNascimento" class="label">Data de Nascimento:</label>
                <asp:TextBox ID="txtDataNascimento" runat="server" placeholder="(dd/mm/aaaa)" CssClass="form-control" onkeypress="formatarData(this)"></asp:TextBox>
            </div>
            <div class="form-group">
                <label for="txtIdEmpresa" class="label">Empresa:</label>
                <div class="input-group">
                    <asp:TextBox ID="txtIdEmpresa" runat="server" placeholder="Digite o código da Empresa" CssClass="form-control" Style="margin-right: 10px;"></asp:TextBox>
                    <div class="input-group-append">
                        <asp:Button ID="btnAdicionaEmpresa" runat="server" Text="Incluir" OnClick="btnAdicionarEmpresa_Click" CssClass="btn btn-primary" />
                    </div>
                </div>
            </div>
            <br />

            <div class="form-group">
                <label for="GridViewEmpresas" class="label">Empresas Relacionadas:</label>
                <asp:GridView ID="GridViewEmpresas" runat="server" AutoGenerateColumns="False" DataKeyNames="Id">
                    <Columns>
                        <asp:BoundField DataField="Id" HeaderText="Id" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="30" />
                        <asp:BoundField DataField="Nome" HeaderText="Nome" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="300"/>
                        <asp:BoundField DataField="Cnpj" HeaderText="CPF" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="150"/>
                        <asp:TemplateField HeaderText="Excluir">
                            <ItemTemplate>
                                <asp:Button ID="btnExcluir" runat="server" Text="Excluir" CommandName="SelectRow" CommandArgument='<%# Container.DataItemIndex %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <br />

            <div class="form-group">
                <label for="GridViewAssociados" class="label">Associados Cadastrados:</label>
                <asp:GridView ID="GridViewAssociados" runat="server" AutoGenerateColumns="False" DataKeyNames="Id">
                    <Columns>
                        <asp:BoundField DataField="Id" HeaderText="Id" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="30" />
                        <asp:BoundField DataField="Nome" HeaderText="Nome" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="300"  />
                        <asp:BoundField DataField="Cpf" HeaderText="CPF" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="150" />
                        <asp:BoundField DataField="DataNascimento" HeaderText="Data de Nascimento" DataFormatString="{0:dd/MM/yyyy}" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="200"/>
                        <asp:TemplateField HeaderText="Selecionar">
                            <ItemTemplate>
                                <asp:Button ID="btnSelecionar" runat="server" Text="Selecionar" CommandName="SelectRow" CommandArgument='<%# Container.DataItemIndex %>' ItemStyle-Width="150" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <br />


            <asp:Button ID="btnNovo" runat="server" Text="Novo" OnClick="btnNovo_Click" CssClass="btn btn-primary" />
            <asp:Button ID="btnGravar" runat="server" Text="Gravar" OnClick="btnGravar_Click" CssClass="btn btn-primary" />
            <asp:Button ID="btnExcluir" runat="server" Text="Excluir" OnClick="btnExcluir_Click" CssClass="btn btn-danger" />
        </div>
    </main>
</asp:Content>
