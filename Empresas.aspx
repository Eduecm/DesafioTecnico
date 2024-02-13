<%@ Page Language="vb" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Empresas.aspx.vb" Inherits="DesafioTecnico.Empresas" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        function formatarCnpj(input) {
            var cnpj = input.value.replace(/\D/g, '');

            if (cnpj.length > 2) {
                cnpj = cnpj.substring(0, 2) + '.' + cnpj.substring(2);
            }
            if (cnpj.length > 6) {
                cnpj = cnpj.substring(0, 6) + '.' + cnpj.substring(6);
            }
            if (cnpj.length > 10) {
                cnpj = cnpj.substring(0, 10) + '/' + cnpj.substring(10);
            }
            if (cnpj.length > 15) {
                cnpj = cnpj.substring(0, 15) + '-' + cnpj.substring(15);
            }

            input.value = cnpj;
        }
    </script>
    <main>
        <div class="container">
            <h1>Empresas</h1>
            <div class="form-group">
                <label for="txtId" class="label">Id:</label>
                <asp:TextBox ID="txtId" runat="server" placeholder="Digite o Id para pesquisar" Text="Novo" CssClass="form-control" Enabled="false"></asp:TextBox>
            </div>
            <table>
                <tr>
                    <td style="padding-right: 10px;">
                        <label for="txtNome" class="label">Nome:</label>
                        <asp:TextBox ID="txtNome" runat="server" CssClass="form-control" MaxLength="200"></asp:TextBox>
                    </td>
                    <td>
                        <label for="txtCnpj" class="label">CNPJ:</label>
                        <asp:TextBox ID="txtCnpj" runat="server" CssClass="form-control" MaxLength="18" onkeypress="formatarCnpj(this)"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <br />

            <div class="form-group">
                <label for="txtIdAssociado" class="label">Associado:</label>
                <div class="input-group">
                    <asp:TextBox ID="txtIdAssociado" runat="server" placeholder="Digite o código do Associado" CssClass="form-control" Style="margin-right: 10px;"></asp:TextBox>
                    <div class="input-group-append">
                        <asp:Button ID="btnAdicionaAssociado" runat="server" Text="Incluir" OnClick="btnAdicionarAssociado_Click" CssClass="btn btn-primary" Enabled="false" />
                    </div>
                </div>
            </div>
            <div class="form-group">
                <label for="GridViewAssociados" class="label">Associados Relacionados:</label>
                <asp:GridView ID="GridViewAssociados" runat="server" AutoGenerateColumns="False" DataKeyNames="Id">
                    <Columns>
                        <asp:BoundField DataField="Id" HeaderText="Id" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="30" />
                        <asp:BoundField DataField="Nome" HeaderText="Nome" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="300" />
                        <asp:BoundField DataField="Cpf" HeaderText="CPF" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="150" />
                        <asp:BoundField DataField="DataNascimento" HeaderText="Data de Nascimento" DataFormatString="{0:dd/MM/yyyy}" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="200" />
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
                <label for="txtPesquisaGrid" class="label">Pesquisar Associados Cadastrados:</label>
                <div class="input-group">
                    <asp:TextBox ID="txtPesquisaGrid" runat="server" CssClass="form-control" Style="margin-right: 10px;"></asp:TextBox>
                    <div class="input-group-append">
                        <asp:Button ID="btnPesquisarGrid" runat="server" Text="Pesquisar" OnClick="btnPesquisarGrid_Click" CssClass="btn btn-primary" />
                        <asp:Button ID="btnLimparPesquisarGrid" runat="server" Text="Limpar" OnClick="btnLimparPesquisarGrid_Click" CssClass="btn btn-primary" />
                    </div>
                </div>
            </div>

            <div class="form-group">
                <label for="GridViewEmpresas" class="label">Empresas Cadastradas:</label>
                <asp:GridView ID="GridViewEmpresas" runat="server" AutoGenerateColumns="False" DataKeyNames="Id">
                    <Columns>
                        <asp:BoundField DataField="Id" HeaderText="Id" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="30" />
                        <asp:BoundField DataField="Nome" HeaderText="Nome" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="300" />
                        <asp:BoundField DataField="Cnpj" HeaderText="CNPJ" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="150" />
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
