Imports System.Data.SqlClient

Public Class Empresas
    Inherits System.Web.UI.Page
    Dim connectionString As String = "Server=EDUARDO\SQLEXPRESS;Database=DesafioTecnico;User Id=sa;Password=123456;Trusted_Connection=True;"
    Dim listaAssociados = New List(Of AssociadoModel)
    Dim listaEmpresas = New List(Of EmpresaModel)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            AtualizaGridEmpresas()
        End If
    End Sub

    Protected Sub btnNovo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnNovo.Click
        LimparCampos()
        AtualizaGridEmpresas()
    End Sub

    Protected Sub btnGravar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGravar.Click

        Dim sql As String
        If txtId.Text = "Novo" Then
            sql = "SELECT Id, Nome, Cnpj FROM Empresas WHERE Cnpj = @cnpj"
            Using con As New SqlConnection(connectionString)
                Using cmd As New SqlCommand(sql, con)
                    cmd.Parameters.AddWithValue("@cnpj", Replace(Replace(Replace(txtCnpj.Text, ".", ""), "-", ""), "/", ""))
                    con.Open()
                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        If reader.Read() Then
                            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "NoDataAlert", $"alert('CNPJ ja cadastrado: {reader("Id")}-{reader("Nome")}, Não é possivel gravar!');", True)
                            Exit Sub
                        End If
                    End Using
                End Using
            End Using
        End If

        If txtId.Text = "Novo" Then
            sql = "INSERT INTO Empresas (Nome, Cnpj) VALUES (@nome, @cnpj) "
        Else
            sql = "UPDATE Empresas SET Nome=@nome, Cnpj=@cnpj WHERE Id=@id "
        End If
        Using con As New SqlConnection(connectionString)
            Using cmd As New SqlCommand(sql, con)
                cmd.Parameters.AddWithValue("@id", txtId.Text)
                cmd.Parameters.AddWithValue("@nome", txtNome.Text)
                cmd.Parameters.AddWithValue("@cnpj", Replace(Replace(Replace(txtCnpj.Text, ".", ""), "-", ""), "/", ""))

                con.Open()
                cmd.ExecuteNonQuery()
            End Using
        End Using

        AtualizaGridEmpresas()

    End Sub

    Protected Sub btnExcluir_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExcluir.Click

        If Not IsNumeric(txtId.Text) Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "NoDataAlert", "alert('Pesquise a Empresa antes de tentar excluir');", True)
            Exit Sub
        End If

        Dim query As String = $"DELETE FROM Empresas WHERE Id = {txtId.Text}"
        Using con As New SqlConnection(connectionString)
            Using cmd As New SqlCommand(query, con)
                con.Open()
                cmd.ExecuteNonQuery()
            End Using
        End Using

        query = $"DELETE FROM Relacionamento WHERE IdEmpresa = {txtId.Text}"
        Using con As New SqlConnection(connectionString)
            Using cmd As New SqlCommand(query, con)
                con.Open()
                cmd.ExecuteNonQuery()
            End Using
        End Using

        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "NoDataAlert", "alert('Empresa excluida com sucesso!');", True)
        LimparCampos()
        AtualizaGridEmpresas()

    End Sub

    Protected Sub btnPesquisar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnPesquisar.Click

        Dim id As String = txtId.Text.Trim()

        If Not IsNumeric(txtId.Text) Then
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "NoDataAlert", "alert('Id inválido ou em branco!');", True)
            Exit Sub
        End If

        PreencherRegistro(id)

    End Sub

    Protected Sub btnAdicionarAssociado_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAdicionaAssociado.Click

        Dim sql As String
        sql = "SELECT Nome FROM Associados WHERE Id = @idAssociado"
        Using con As New SqlConnection(connectionString)
            Using cmd As New SqlCommand(sql, con)
                cmd.Parameters.AddWithValue("@idAssociado", txtIdAssociado.Text)
                con.Open()
                Using reader As SqlDataReader = cmd.ExecuteReader()
                    If Not reader.Read() Then
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "NoDataAlert", $"alert('Associado inexistente!');", True)
                        Exit Sub
                    End If
                End Using
            End Using
        End Using

        sql = "SELECT IdEmpresa, IdAssociado FROM Relacionamento WHERE IdEmpresa = @idEmpresa AND IdAssociado = @idAssociado"
        Using con As New SqlConnection(connectionString)
            Using cmd As New SqlCommand(sql, con)
                cmd.Parameters.AddWithValue("@IdEmpresa", txtId.Text)
                cmd.Parameters.AddWithValue("@IdAssociado", txtIdAssociado.Text)
                con.Open()
                Using reader As SqlDataReader = cmd.ExecuteReader()
                    If reader.Read() Then
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "NoDataAlert", $"alert('Associado já vinculado!');", True)
                        Exit Sub
                    End If
                End Using
            End Using
        End Using

        sql = "INSERT INTO Relacionamento (idEmpresa, idAssociado) VALUES (@idEmpresa, @idAssociado) "
        Using con As New SqlConnection(connectionString)
            Using cmd As New SqlCommand(sql, con)
                cmd.Parameters.AddWithValue("@idEmpresa", txtId.Text)
                cmd.Parameters.AddWithValue("@idAssociado", txtIdAssociado.Text)

                con.Open()
                cmd.ExecuteNonQuery()
            End Using
        End Using

        txtIdAssociado.Text = ""
        txtIdAssociado.Focus()
        AtualizaGridAssociados()

    End Sub

    Protected Sub GridViewEmpresas_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles GridViewEmpresas.RowCommand
        If e.CommandName = "SelectRow" Then
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            GridViewEmpresas.SelectedIndex = index
            Dim selectedRow As GridViewRow = GridViewEmpresas.Rows(index)
            PreencherRegistro(selectedRow.Cells(0).Text)
        End If
    End Sub

    Protected Sub GridViewAssociados_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles GridViewAssociados.RowCommand
        If e.CommandName = "SelectRow" Then
            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            GridViewAssociados.SelectedIndex = index
            Dim selectedRow As GridViewRow = GridViewAssociados.Rows(index)
            DeletarAssociado(selectedRow.Cells(0).Text)
        End If
    End Sub

    Private Sub DeletarAssociado(ByVal idAssociado As Integer)

        Dim sql As String = $"DELETE FROM Relacionamento WHERE IdAssociado = {idAssociado} AND IdEmpresa = {txtId.Text}"
        Using con As New SqlConnection(connectionString)
            Using cmd As New SqlCommand(sql, con)
                con.Open()
                cmd.ExecuteNonQuery()
            End Using
        End Using

        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "NoDataAlert", "alert('Associado excluido com sucesso!');", True)
        AtualizaGridAssociados()

    End Sub

    Private Sub AtualizaGridEmpresas()
        Dim sql As String
        sql = "SELECT Id, Nome, Cnpj FROM Empresas ORDER BY Nome"
        Using con As New SqlConnection(connectionString)
            Using cmd As New SqlCommand(sql, con)
                con.Open()
                Using reader As SqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        Dim empresa As New EmpresaModel With {
                            .Id = reader("Id").ToString(),
                            .Nome = reader("Nome").ToString(),
                            .CNPJ = FormatCpfCnpj(reader("Cnpj").ToString())
                        }
                        listaEmpresas.Add(empresa)
                    End While
                End Using
            End Using
        End Using

        GridViewEmpresas.DataSource = listaEmpresas
        GridViewEmpresas.DataBind()
    End Sub

    Private Sub AtualizaGridAssociados()
        Dim sql As String
        sql = $"SELECT Associados.Id, Associados.Nome, Associados.CPF, Associados.DataNascimento
        FROM Relacionamento
        LEFT JOIN Associados ON Relacionamento.idAssociado = Associados.Id
        WHERE Relacionamento.idEmpresa = {txtId.Text}
        ORDER BY Nome"
        Using con As New SqlConnection(connectionString)
            Using cmd As New SqlCommand(sql, con)
                con.Open()
                Using reader As SqlDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        Dim associado As New AssociadoModel With {
                            .Id = reader("Id").ToString(),
                            .Nome = reader("Nome").ToString(),
                            .CPF = FormatCpfCnpj(reader("CPF").ToString()),
                            .DataNascimento = Left(reader("DataNascimento").ToString(), 10)
                        }
                        listaAssociados.Add(associado)
                    End While
                End Using
            End Using
        End Using

        GridViewAssociados.DataSource = listaAssociados
        GridViewAssociados.DataBind()
    End Sub

    Private Sub LimparCampos()

        txtId.Text = "Novo"
        txtNome.Text = ""
        txtCnpj.Text = ""
        GridViewAssociados.DataSource = ""
        GridViewAssociados.DataBind()

    End Sub

    Protected Sub PreencherRegistro(ByVal id As Integer)
        Dim query As String = "SELECT Id, Nome, Cnpj FROM Empresas WHERE Id = @id"
        Using con As New SqlConnection(connectionString)
            Using cmd As New SqlCommand(query, con)
                cmd.Parameters.AddWithValue("@id", id)
                con.Open()
                Using reader As SqlDataReader = cmd.ExecuteReader()
                    If reader.Read() Then
                        txtId.Text = reader("Id").ToString()
                        txtNome.Text = reader("Nome").ToString()
                        txtCnpj.Text = FormatCpfCnpj(reader("Cnpj").ToString())
                        AtualizaGridAssociados()
                    Else
                        LimparCampos()
                        AtualizaGridEmpresas()
                        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "NoDataAlert", "alert('Nenhuma empresa encontrado.');", True)
                    End If
                End Using
            End Using
        End Using
    End Sub

    Protected Function FormatCpfCnpj(ByVal cpfcnpj As Object) As String
        If Not String.IsNullOrEmpty(cpfcnpj) Then
            Dim cpfcnpjString As String = Replace(Replace(Replace(cpfcnpj.ToString(), ".", ""), "-", ""), "/", "")
            If Len(cpfcnpjString) > 11 Then
                Return String.Format("{0}.{1}.{2}/{3}-{4}", cpfcnpjString.Substring(0, 2), cpfcnpjString.Substring(2, 3), cpfcnpjString.Substring(5, 3), cpfcnpjString.Substring(8, 4), cpfcnpjString.Substring(12, 2))
            ElseIf Len(cpfcnpjString) = 11 Then
                Return String.Format("{0}.{1}.{2}-{3}", cpfcnpjString.Substring(0, 3), cpfcnpjString.Substring(3, 3), cpfcnpjString.Substring(6, 3), cpfcnpjString.Substring(9, 2))
            Else
                Return String.Empty
            End If
        Else
            Return String.Empty
        End If
    End Function
End Class