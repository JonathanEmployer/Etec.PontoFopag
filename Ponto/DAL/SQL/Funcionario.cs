using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Collections.Generic;
using AutoMapper;
using System.Linq;
using Modelo.Proxy;
using static Modelo.Enumeradores;

namespace DAL.SQL
{
    public class Funcionario : DAL.SQL.DALBase, DAL.IFuncionario
    {
        private DAL.SQL.MudCodigoFunc _dalMudanca;

        public DAL.SQL.MudCodigoFunc dalMudanca
        {
            get { return _dalMudanca; }
            set { _dalMudanca = value; }
        }

        private DAL.SQL.FuncionarioHistorico _dalFuncionarioHistorico;

        public DAL.SQL.FuncionarioHistorico dalFuncionarioHistorico
        {
            get { return _dalFuncionarioHistorico; }
            set { _dalFuncionarioHistorico = value; }
        }

        private DAL.SQL.Biometria _dalBiometrias;
        public DAL.SQL.Biometria dalBiometrias
        {
            get { return _dalBiometrias; }
            set { _dalBiometrias = value; }
        }

        private string SELECTPCONT;
        private string SELECTCONT;
        public string SELECTREL
        {
            get
            {
                return @"   SELECT   func.id
                                    , CAST(func.dscodigo AS BIGINT) AS codigo 
                                    , func.nome
                                    , func.IdAlocacao
                                    , func.IdTipoVinculo
                                    , func.matricula matricula                                   
                                    , ISNULL (func.dataadmissao,' ') AS dataadmissao
                                    , ISNULL (func.datademissao,' ') AS datademissao
                                    , convert(varchar,emp.codigo)+' | '+emp.nome as empresa
                                    , convert(varchar,dep.codigo)+' | '+dep.descricao as departamento
                                    , convert(varchar,funcao.codigo) + ' | ' + funcao.descricao AS funcao
                                    , coalesce(convert(varchar,cwu.codigo) + ' | ' + cwu.nome, '') AS supervisor
									, coalesce(convert(varchar, pe.codigo) + ' | ' + pe.razaosocial, '') as PessoaSupervisor
                                    , func.CPF
                                    , func.Mob_Senha
                                    , convert(varchar,hor.codigo)+' | '+hor.descricao as horario
                                    , convert(varchar,Alocacao.codigo) + ' | ' + Alocacao.descricao AS Alocacao
                                    , convert(varchar,TipoVinculo.codigo) + ' | ' + TipoVinculo.descricao AS TipoVinculo
                                    , (SELECT TOP 1 convert(varchar,hd.codigo) + ' | ' + ISNULL(hd.entrada_1, '--:--') + ' - ' + ISNULL(hd.saida_1, '--:--') + ' | ' + ISNULL(hd.entrada_2, '--:--') + ' - ' + ISNULL(hd.saida_2, '--:--') 
                                    + ' | ' + ISNULL(hd.entrada_3, '--:--') + ' - ' + ISNULL(hd.saida_3, '--:--') + ' | ' + ISNULL(hd.entrada_4, '--:--') + ' - ' + ISNULL(hd.saida_4, '--:--')
                                      FROM horariodetalhe hd WHERE hd.idhorario = hor.id AND (hd.dia = 1 OR DATEPART(WEEKDAY, hd.data) = 2)) AS horariodescricao
                             FROM funcionario func
                             LEFT JOIN empresa emp ON emp.id = func.idempresa
                             LEFT JOIN departamento dep ON dep.id = func.iddepartamento
                             LEFT JOIN horario hor ON hor.id = func.idhorario
                             LEFT JOIN funcao ON funcao.id = func.idFuncao
                             LEFT JOIN cw_usuario cwu ON cwu.id = func.idcw_usuario
							 LEFT JOIN pessoa pe ON pe.id = func.IdPessoaSupervisor
                             LEFT JOIN Alocacao ON alocacao.id = func.idAlocacao
                             LEFT JOIN TipoVinculo ON TipoVinculo.id = func.idTipoVinculo
                             WHERE ISNULL(func.excluido, 0) = 0 ";
            }
        }

        public string SELECTPRO 
        {
            get 
            {
                return @"   SELECT    func.id
                                    , func.codigo
                                    , ISNULL(func.nome, '') AS nome
                                    , func.dscodigo
                                    , func.matricula AS matricula                                    
                                    , horario.descricao AS jornada
                                    , convert(varchar,emp.codigo)+' | '+emp.nome as empresa
                                    , convert(varchar,departamento.codigo)+' | '+departamento.descricao AS departamento
                                    , convert(varchar,funcao.codigo) + ' | ' + funcao.descricao AS funcao
                                    , coalesce(convert(varchar,cwu.codigo) + ' | ' + cwu.nome, '') AS supervisor
                                    , coalesce(convert(varchar, pe.codigo) + ' | ' + pe.razaosocial, '') as PessoaSupervisor
                                    , convert(varchar,Alocacao.codigo) + ' | ' + Alocacao.descricao AS Alocacao
                                    , convert(varchar,TipoVinculo.codigo) + ' | ' + TipoVinculo.descricao AS TipoVinculo
                                    , func.carteira
                                    , func.dataadmissao
                                    , func.CPF
                                    , func.Mob_Senha
                                    , func.TipoMaoObra
                                    , func.IdAlocacao
                                    , func.IdTipoVinculo
                                    , func.IdIntegracaoPainel
                                    , func.Email                                
                             FROM funcionario func
                             LEFT JOIN empresa emp ON emp.id = func.idempresa
                             LEFT JOIN departamento ON departamento.id = func.iddepartamento
                             LEFT JOIN horario ON horario.id = func.idhorario
                             LEFT JOIN funcao ON funcao.id = func.idFuncao
                             LEFT JOIN cw_usuario cwu ON cwu.id = func.idcw_usuario
                             LEFT JOIN pessoa pe ON pe.id = func.IdPessoaSupervisor
                             LEFT JOIN Alocacao ON alocacao.id = func.idAlocacao
                             LEFT JOIN TipoVinculo ON TipoVinculo.id = func.idTipoVinculo
                             WHERE ISNULL(func.excluido, 0) = 0  AND ISNULL(func.funcionarioativo, 0) = 1 
                             ORDER BY LOWER(func.nome)";
            }
        }

        public string SELECTDEL
        {
            get
            {
                return @"   SELECT    func.id
                                    , func.nome
                                    , CAST(func.dscodigo AS BIGINT) AS dscodigo
                                    , func.matricula AS matricula                                    
                                    , horario.descricao AS jornada
                                    , convert(varchar,emp.codigo)+' | '+emp.nome as empresa
                                    , convert(varchar,departamento.codigo)+' | '+departamento.descricao AS departamento
                                    , convert(varchar,funcao.codigo) + ' | ' + funcao.descricao AS funcao
                                    , coalesce(convert(varchar,cwu.codigo) + ' | ' + cwu.nome, '') AS supervisor
                                    , coalesce(convert(varchar, pe.codigo) + ' | ' + pe.razaosocial, '') as PessoaSupervisor
                                    , convert(varchar,Alocacao.codigo) + ' | ' + Alocacao.descricao AS Alocacao
                                    , convert(varchar,TipoVinculo.codigo) + ' | ' + TipoVinculo.descricao AS TipoVinculo
                                    , func.carteira
                                    , func.dataadmissao
                                    , func.CPF
                                    , func.Mob_Senha
                                    , func.TipoMaoObra
                                    , func.IdAlocacao
                                    , func.IdTipoVinculo
                                    , func.IdIntegracaoPainel
                                    , func.Email                                 
                             FROM funcionario func
                             LEFT JOIN empresa emp ON emp.id = func.idempresa
                             LEFT JOIN departamento ON departamento.id = func.iddepartamento
                             LEFT JOIN horario ON horario.id = func.idhorario
                             LEFT JOIN funcao ON funcao.id = func.idFuncao
                             LEFT JOIN cw_usuario cwu ON cwu.id = func.idcw_usuario
                             LEFT JOIN pessoa pe ON pe.id = func.IdPessoaSupervisor
                             LEFT JOIN Alocacao ON alocacao.id = func.idAlocacao
                             LEFT JOIN TipoVinculo ON TipoVinculo.id = func.idTipoVinculo
                             WHERE ISNULL(func.excluido, 0) = 1 ";
            }
        }

        protected override string SELECTALL
        {
            get
                {
                return @"   SELECT   func.id
                                    , func.nome
                                    , CAST(func.dscodigo AS BIGINT) AS dscodigo 
                                    , func.matricula AS matricula                                    
                                    , horario.descricao AS jornada
                                    , convert(varchar,emp.codigo)+' | '+emp.nome as empresa
                                    , convert(varchar,departamento.codigo)+' | '+departamento.descricao AS departamento
                                    , convert(varchar,funcao.codigo) + ' | ' + funcao.descricao AS funcao
                                    , coalesce(convert(varchar,cwu.codigo) + ' | ' + cwu.nome, '') AS supervisor
                                    , coalesce(convert(varchar, pe.codigo) + ' | ' + pe.razaosocial, '') as PessoaSupervisor
                                    , convert(varchar,Alocacao.codigo) + ' | ' + Alocacao.descricao AS Alocacao
                                    , convert(varchar,TipoVinculo.codigo) + ' | ' + TipoVinculo.descricao AS TipoVinculo
                                    , func.carteira AS carteira
                                    , func.dataadmissao
                                    , func.funcionarioativo
                                    , func.DataInativacao
                                    , func.CPF
                                    , func.TipoMaoObra
                                    , func.IdAlocacao
                                    , func.IDTipoVinculo
                                    , func.IdIntegracaoPainel
                                    , func.Email                                  
                             FROM funcionario func
                             LEFT JOIN empresa emp ON emp.id = func.idempresa
                             LEFT JOIN departamento ON departamento.id = func.iddepartamento
                             LEFT JOIN horario ON horario.id = func.idhorario
                             LEFT JOIN funcao ON funcao.id = func.idFuncao
                             LEFT JOIN cw_usuario cwu ON cwu.id = func.idcw_usuario
                             LEFT JOIN pessoa pe ON pe.id = func.IdPessoaSupervisor
                             LEFT JOIN Alocacao ON alocacao.id = func.idAlocacao
                             LEFT JOIN TipoVinculo ON TipoVinculo.id = func.idTipoVinculo
                             WHERE ISNULL(func.excluido, 0) = 0 
                             ORDER BY func.nome";
            }
            set
            {
                base.SELECTALL = value;
            }
        }

        public string SqlLoadByPis()
        {
            return @"
                                    SELECT   func.id,
       func.codigo,
       func.dscodigo,
       func.matricula,
       func.nome,
       func.codigofolha,
       func.idempresa,
       func.iddepartamento,
       func.idfuncao,
       func.idhorario,
       func.tipohorario,
       func.carteira,
       func.dataadmissao,
       func.datademissao,
       func.salario,
       func.funcionarioativo,
       func.DataInativacao,
       func.naoentrarbanco,
       func.naoentrarcompensacao,
       func.excluido,
       func.campoobservacao,
       func.incdata,
       func.inchora,
       func.incusuario,
       func.altdata,
       func.althora,
       func.altusuario,
       func.pis,
       func.senha,
       func.toleranciaentrada,
       func.toleranciasaida,
       func.quantidadetickets,
       func.tipotickets,
       func.CPF,
       func.Mob_Senha,
       func.idcw_usuario,
       func.utilizaregistrador,
       func.UtilizaAppPontofopag,
       func.UtilizaReconhecimentoFacialApp,
       func.UtilizaWebAppPontofopag,
       func.UtilizaReconhecimentoFacialWebApp,
       func.idIntegracao,
       func.IdPessoaSupervisor,
       func.TipoMaoObra,
       func.IdAlocacao,
       func.IdTipoVinculo,
       func.Email,
       func.IdIntegracaoPainel,
       func.RFID, 
        '' foto,
        '' contrato
                                    , horario.descricao AS jornada
                                    , convert(varchar,empresa.codigo)+' | '+empresa.nome as empresa
                                    , convert(varchar,departamento.codigo)+' | '+departamento.descricao AS departamento
                                    , convert(varchar,funcao.codigo) + ' | ' + funcao.descricao AS funcao
                                    , coalesce(convert(varchar,cwu.codigo) + ' | ' + cwu.nome, '') AS supervisor
                                    , coalesce(convert(varchar, pe.codigo) + ' | ' + pe.razaosocial, '') as PessoaSupervisor
                                    , convert(varchar,Alocacao.codigo) + ' | ' + Alocacao.descricao AS Alocacao
                                    , convert(varchar,TipoVinculo.codigo) + ' | ' + TipoVinculo.descricao AS TipoVinculo
                             FROM funcionario func 
                             LEFT JOIN empresa ON empresa.id = func.idempresa
                             LEFT JOIN departamento ON departamento.id = func.iddepartamento
                             LEFT JOIN horario ON horario.id = func.idhorario
                             LEFT JOIN funcao ON funcao.id = func.idFuncao
                             LEFT JOIN cw_usuario cwu ON cwu.id = func.idcw_usuario
                             LEFT JOIN pessoa pe ON pe.id = func.IdPessoaSupervisor
                             LEFT JOIN Alocacao ON alocacao.id = func.idAlocacao
                             LEFT JOIN TipoVinculo ON TipoVinculo.id = func.idTipoVinculo
                             WHERE convert(BIGINT,isnull(replace(replace(func.pis,'.',''),'-',''),0)) = convert(BIGINT,isnull((replace(replace(@PIS,'.',''),'-','')),0))
                        ";
        }

        public Funcionario(DataBase database)
        {
            db = database;
            dalMudanca = new DAL.SQL.MudCodigoFunc(db);
            dalFuncionarioHistorico = new DAL.SQL.FuncionarioHistorico(db);
            dalBiometrias = new DAL.SQL.Biometria(db);
            TABELA = "funcionario";

            SELECTPID = @"   SELECT   func.id,
                                       func.codigo,
                                       func.dscodigo,
                                       func.matricula,
                                       func.nome,
                                       func.codigofolha,
                                       func.idempresa,
                                       func.iddepartamento,
                                       func.idfuncao,
                                       func.idhorario,
                                       func.tipohorario,
                                       func.carteira,
                                       uContr.contrato,
                                       func.dataadmissao,
                                       func.datademissao,
                                       func.salario,
                                       func.funcionarioativo,
                                       func.DataInativacao,
                                       func.naoentrarbanco,
                                       func.naoentrarcompensacao,
                                       func.excluido,
                                       func.campoobservacao,
                                       func.incdata,
                                       func.inchora,
                                       func.incusuario,
                                       func.altdata,
                                       func.althora,
                                       func.altusuario,
                                       func.pis,
                                       func.senha,
                                       func.toleranciaentrada,
                                       func.toleranciasaida,
                                       func.quantidadetickets,
                                       func.tipotickets,
                                       func.CPF,
                                       func.Mob_Senha,
                                       func.idcw_usuario,
                                       func.utilizaregistrador,
                                       func.UtilizaAppPontofopag,
                                       func.UtilizaReconhecimentoFacialApp,
                                       func.UtilizaWebAppPontofopag,
                                       func.UtilizaReconhecimentoFacialWebApp,
                                       func.idIntegracao,
                                       func.IdPessoaSupervisor,
                                       func.TipoMaoObra,
                                       func.IdAlocacao,
                                       func.IdTipoVinculo,
                                       func.Email,
                                       func.IdIntegracaoPainel,
                                       func.RFID,
                                       func.foto
                                    , convert(varchar,horario.codigo)+' | '+horario.descricao AS jornada
                                    , convert(varchar,empresa.codigo)+' | '+empresa.nome as empresa
                                    , convert(varchar,departamento.codigo)+' | '+departamento.descricao AS departamento
                                    , convert(varchar,funcao.codigo) + ' | ' + funcao.descricao AS funcao
                                    , coalesce(convert(varchar,cwu.codigo) + ' | ' + cwu.nome, '') AS supervisor
                                    , coalesce(convert(varchar, pe.codigo) + ' | ' + pe.razaosocial, '') as PessoaSupervisor
                                    , convert(varchar,Alocacao.codigo) + ' | ' + Alocacao.descricao AS Alocacao
                                    , convert(varchar,TipoVinculo.codigo) + ' | ' + TipoVinculo.descricao AS TipoVinculo
                                    , func.IdHorarioDinamico
                                    , func.CicloSequenciaIndice
                                    , convert(varchar,HDN.codigo) +' | '+HDN.descricao HorarioDinamico
                             FROM funcionario func 
                             LEFT JOIN empresa ON empresa.id = func.idempresa
                             LEFT JOIN departamento ON departamento.id = func.iddepartamento
                             LEFT JOIN horario ON horario.id = func.idhorario
                             LEFT JOIN funcao ON funcao.id = func.idFuncao
                             LEFT JOIN cw_usuario cwu ON cwu.id = func.idcw_usuario
                             LEFT JOIN pessoa pe ON pe.id = func.IdPessoaSupervisor
                             LEFT JOIN Alocacao ON alocacao.id = func.idAlocacao
                             LEFT JOIN TipoVinculo ON TipoVinculo.id = func.idTipoVinculo
                             LEFT JOIN HorarioDinamico HDN ON HDN.id = func.idhorariodinamico
                             OUTER APPLY(SELECT TOP 1 cfun.inchora,CASE WHEN cont.codigo is null THEN '-' ELSE CONCAT(cont.codigo,' | ',cont.codigocontrato,' - ',cont.descricaocontrato) END contrato FROM dbo.contratofuncionario cfun LEFT JOIN dbo.contrato cont ON cont.id = cfun.idcontrato 
												WHERE func.id = cfun.idfuncionario and cfun.excluido =0 ) AS uContr                                
                             WHERE func.id = @id";
            SELECTPCPF = @"   SELECT   func.id,
                                       func.codigo,
                                       func.dscodigo,
                                       func.matricula,
                                       func.nome,
                                       func.codigofolha,
                                       func.idempresa,
                                       func.iddepartamento,
                                       func.idfuncao,
                                       func.idhorario,
                                       func.tipohorario,
                                       func.carteira,
                                       uContr.contrato,
                                       func.dataadmissao,
                                       func.datademissao,
                                       func.salario,
                                       func.funcionarioativo,
                                       func.DataInativacao,
                                       func.naoentrarbanco,
                                       func.naoentrarcompensacao,
                                       func.excluido,
                                       func.campoobservacao,
                                       func.incdata,
                                       func.inchora,
                                       func.incusuario,
                                       func.altdata,
                                       func.althora,
                                       func.altusuario,
                                       func.pis,
                                       func.senha,
                                       func.toleranciaentrada,
                                       func.toleranciasaida,
                                       func.quantidadetickets,
                                       func.tipotickets,
                                       func.CPF,
                                       func.Mob_Senha,
                                       func.idcw_usuario,
                                       func.utilizaregistrador,
                                       func.UtilizaAppPontofopag,
                                       func.UtilizaReconhecimentoFacialApp,
                                       func.UtilizaWebAppPontofopag,
                                       func.UtilizaReconhecimentoFacialWebApp,
                                       func.idIntegracao,
                                       func.IdPessoaSupervisor,
                                       func.TipoMaoObra,
                                       func.IdAlocacao,
                                       func.IdTipoVinculo,
                                       func.Email,
                                       func.IdIntegracaoPainel,
                                       func.RFID,  
                                       '' foto
                                    , horario.descricao AS jornada
                                    , convert(varchar,empresa.codigo)+' | '+empresa.nome as empresa
                                    , convert(varchar,departamento.codigo)+' | '+departamento.descricao AS departamento
                                    , convert(varchar,funcao.codigo) + ' | ' + funcao.descricao AS funcao
                                    , coalesce(convert(varchar,cwu.codigo) + ' | ' + cwu.nome, '') AS supervisor
                                    , coalesce(convert(varchar, pe.codigo) + ' | ' + pe.razaosocial, '') as PessoaSupervisor
                                    , convert(varchar,Alocacao.codigo) + ' | ' + Alocacao.descricao AS Alocacao
                                    , convert(varchar,TipoVinculo.codigo) + ' | ' + TipoVinculo.descricao AS TipoVinculo
                                    , func.IdHorarioDinamico
                                    , func.CicloSequenciaIndice
                                    , convert(varchar,HDN.codigo) +' | '+HDN.descricao HorarioDinamico
                             FROM funcionario func 
                             LEFT JOIN empresa ON empresa.id = func.idempresa
                             LEFT JOIN departamento ON departamento.id = func.iddepartamento
                             LEFT JOIN horario ON horario.id = func.idhorario
                             LEFT JOIN funcao ON funcao.id = func.idFuncao
                             LEFT JOIN cw_usuario cwu ON cwu.id = func.idcw_usuario
                             LEFT JOIN pessoa pe ON pe.id = func.IdPessoaSupervisor
                             LEFT JOIN Alocacao ON alocacao.id = func.idAlocacao
                             LEFT JOIN TipoVinculo ON TipoVinculo.id = func.idTipoVinculo
                             LEFT JOIN HorarioDinamico HDN ON HDN.id = func.idhorariodinamico
                            OUTER APPLY(SELECT TOP 1 cfun.inchora,CASE WHEN cont.codigo is null THEN '-' ELSE CONCAT(cont.codigo,' | ',cont.codigocontrato,' - ',cont.descricaocontrato) END contrato FROM dbo.contratofuncionario cfun LEFT JOIN dbo.contrato cont ON cont.id = cfun.idcontrato 
												WHERE func.id = cfun.idfuncionario and cfun.excluido =0 ) AS uContr
                             WHERE convert(BIGINT,isnull(replace(replace(func.CPF,'.',''),'-',''),0)) = convert(BIGINT,isnull((replace(replace(@CPF,'.',''),'-','')),0))";

            SELECTCONT = @"
                            select 
	                                    f.id,
                                       f.codigo,
                                       f.dscodigo,
                                       f.matricula,
                                       f.nome,
                                       f.codigofolha,
                                       f.idempresa,
                                       f.iddepartamento,
                                       f.idfuncao,
                                       f.idhorario,
                                       f.tipohorario,
                                       f.carteira,
                                       f.dataadmissao,
                                       f.datademissao,
                                       f.salario,
                                       f.funcionarioativo,
                                       f.DataInativacao,
                                       f.naoentrarbanco,
                                       f.naoentrarcompensacao,
                                       f.excluido,
                                       f.campoobservacao,
                                       f.incdata,
                                       f.inchora,
                                       f.incusuario,
                                       f.altdata,
                                       f.althora,
                                       f.altusuario,
                                       f.pis,
                                       f.senha,
                                       f.toleranciaentrada,
                                       f.toleranciasaida,
                                       f.quantidadetickets,
                                       f.tipotickets,
                                       f.CPF,
                                       f.Mob_Senha,
                                       f.idcw_usuario,
                                       f.utilizaregistrador,
                                       f.UtilizaAppPontofopag,
                                       f.UtilizaReconhecimentoFacialApp,
                                       f.UtilizaWebAppPontofopag,
                                       f.UtilizaReconhecimentoFacialWebApp,
                                       f.idIntegracao,
                                       f.IdPessoaSupervisor,
                                       f.TipoMaoObra,
                                       f.IdAlocacao,
                                       f.IdTipoVinculo,
                                       f.Email,
                                       f.IdIntegracaoPainel,
                                       f.RFID, 
                                        '' foto
	                            , CONVERT(varchar, emp.codigo) + ' | ' + emp.nome as NomeEmpresa  
	                            , ct.codigocontrato as CodigoContrato
	                            , CONVERT(varchar, f.codigo) + ' | ' + f.nome as NomeFuncionario
                            from contratofuncionario cf
                            join contrato ct on cf.idcontrato = ct.id
                            join empresa emp on ct.idempresa = emp.id
                            join funcionario f on cf.idfuncionario = f.id ";

            SELECTPCONT = @"
                            select 
		                            f.id,
                                       f.codigo,
                                       f.dscodigo,
                                       f.matricula,
                                       f.nome,
                                       f.codigofolha,
                                       f.idempresa,
                                       f.iddepartamento,
                                       f.idfuncao,
                                       f.idhorario,
                                       f.tipohorario,
                                       f.carteira,
                                       f.dataadmissao,
                                       f.datademissao,
                                       f.salario,
                                       f.funcionarioativo,
                                       f.DataInativacao,
                                       f.naoentrarbanco,
                                       f.naoentrarcompensacao,
                                       f.excluido,
                                       f.campoobservacao,
                                       f.incdata,
                                       f.inchora,
                                       f.incusuario,
                                       f.altdata,
                                       f.althora,
                                       f.altusuario,
                                       f.pis,
                                       f.senha,
                                       f.toleranciaentrada,
                                       f.toleranciasaida,
                                       f.quantidadetickets,
                                       f.tipotickets,
                                       f.CPF,
                                       f.Mob_Senha,
                                       f.idcw_usuario,
                                       f.utilizaregistrador,
                                       f.UtilizaAppPontofopag,
                                       f.UtilizaReconhecimentoFacialApp,
                                       f.UtilizaWebAppPontofopag,
                                       f.UtilizaReconhecimentoFacialWebApp,
                                       f.idIntegracao,
                                       f.IdPessoaSupervisor,
                                       f.TipoMaoObra,
                                       f.IdAlocacao,
                                       f.IdTipoVinculo,
                                       f.Email,
                                       f.IdIntegracaoPainel,
                                       f.RFID, 
                                        '' foto
		                            , CONVERT(varchar, emp.codigo) + ' | ' + emp.nome as NomeEmpresa  
		                            , ct.codigocontrato as CodigoContrato
		                            , CONVERT(varchar, f.codigo) + ' | ' + f.nome as NomeFuncionario
	                            from contratofuncionario cf
	                            join contrato ct on cf.idcontrato = ct.id
                                join empresa emp on ct.idempresa = emp.id
	                            join funcionario f on cf.idfuncionario = f.id
                            WHERE ct.id = @idcontrato and cf.excluido = 0";

            INSERT = @"  INSERT INTO funcionario
							( codigo,  dscodigo,  matricula,  nome,  codigofolha,  idempresa,  iddepartamento,  idfuncao,  idhorario,  tipohorario,  carteira,  dataadmissao,  datademissao,  salario,  naoentrarbanco,  naoentrarcompensacao,  excluido,  campoobservacao,  foto,  incdata,  inchora,  incusuario,  pis,  senha,  toleranciaentrada,  toleranciasaida,  quantidadetickets,  tipotickets,  CPF,  Mob_Senha,  idcw_usuario,  utilizaregistrador,  idIntegracao,  IdPessoaSupervisor,  TipoMaoObra,  IdAlocacao,  IdTipoVinculo,  IdIntegracaoPainel,  Email,  RFID,  IdHorarioDinamico,  CicloSequenciaIndice,  DataInativacao,  UtilizaAppPontofopag,  UtilizaReconhecimentoFacialApp,  UtilizaWebAppPontofopag,  UtilizaReconhecimentoFacialWebApp)
							VALUES
							(@codigo, @dscodigo, @matricula, @nome, @codigofolha, @idempresa, @iddepartamento, @idfuncao, @idhorario, @tipohorario, @carteira, @dataadmissao, @datademissao, @salario, @naoentrarbanco, @naoentrarcompensacao, @excluido, @campoobservacao, @foto, @incdata, @inchora, @incusuario, @pis, @senha, @toleranciaentrada, @toleranciasaida, @quantidadetickets, @tipotickets, @CPF, @Mob_Senha, @idcw_usuario, @utilizaregistrador, @idIntegracao, @IdPessoaSupervisor, @TipoMaoObra, @IdAlocacao, @IdTipoVinculo, @IdIntegracaoPainel, @Email, @RFID, @IdHorarioDinamico, @CicloSequenciaIndice, @DataInativacao, @UtilizaAppPontofopag, @UtilizaReconhecimentoFacialApp, @UtilizaWebAppPontofopag, @UtilizaReconhecimentoFacialWebApp)
						SET @id = SCOPE_IDENTITY()";

            UPDATE = @"  UPDATE funcionario SET codigo = @codigo
							, dscodigo = @dscodigo
							, matricula = @matricula
							, nome = @nome
							, codigofolha = @codigofolha
							, idempresa = @idempresa
							, iddepartamento = @iddepartamento
							, idfuncao = @idfuncao
							, idhorario = @idhorario
							, tipohorario = @tipohorario
							, carteira = @carteira
							, dataadmissao = @dataadmissao
							, datademissao = @datademissao
							, salario = @salario
							, naoentrarbanco = @naoentrarbanco
							, naoentrarcompensacao = @naoentrarcompensacao
							, excluido = @excluido
							, campoobservacao = @campoobservacao
							, foto = @foto
							, altdata = @altdata
							, althora = @althora
							, altusuario = @altusuario
                            , pis = @pis
                            , senha = @senha
                            , toleranciaentrada = @toleranciaentrada
                            , toleranciasaida = @toleranciasaida
                            , quantidadetickets = @quantidadetickets
                            , tipotickets = @tipotickets
                            , CPF = @CPF
                            , Mob_Senha = @Mob_Senha
                            , idcw_usuario = @idcw_usuario
                            , utilizaregistrador = @utilizaregistrador
                            , idIntegracao = @idIntegracao
                            , IdPessoaSupervisor = @IdPessoaSupervisor
                            , TipoMaoObra = @TipoMaoObra
                            , IdAlocacao = @IdAlocacao
                            , IdTipoVinculo = @IdTipoVinculo
                            , IdIntegracaoPainel = @IdIntegracaoPainel
                            , Email = @Email
                            , RFID = @RFID
                            , IdHorarioDinamico = @IdHorarioDinamico
                            , CicloSequenciaIndice = @CicloSequenciaIndice
                            , DataInativacao = @DataInativacao
                            , UtilizaAppPontofopag = @UtilizaAppPontofopag
                            , UtilizaReconhecimentoFacialApp = @UtilizaReconhecimentoFacialApp
                            , UtilizaWebAppPontofopag = @UtilizaWebAppPontofopag
                            , UtilizaReconhecimentoFacialWebApp = @UtilizaReconhecimentoFacialWebApp
						WHERE id = @id";

            DELETE = @"  DELETE FROM funcionario WHERE id = @id";

            MAXCOD = @" SELECT  CASE WHEN codigo > dscodigo  or dscodigo > 2147483647 THEN codigo
                                     ELSE dscodigo
                                END as codigo
                        FROM    ( SELECT    ISNULL(MAX(codigo),0) codigo ,
                                            ISNULL(MAX(CONVERT(BIGINT,ISNULL(dscodigo,0))),0) dscodigo
                                  FROM      dbo.funcionario
                                ) t ";

        }

        public string SqlLoadByCodigo()
        {
            return @"   SELECT   func.*
                                    , convert(varchar,horario.codigo)+' | '+horario.descricao AS jornada
                                    , convert(varchar,empresa.codigo)+' | '+empresa.nome as empresa
                                    , convert(varchar,departamento.codigo)+' | '+departamento.descricao AS departamento
                                    , convert(varchar,funcao.codigo) + ' | ' + funcao.descricao AS funcao
                                    , coalesce(convert(varchar,cwu.codigo) + ' | ' + cwu.nome, '') AS supervisor
                                    , coalesce(convert(varchar, pe.codigo) + ' | ' + pe.razaosocial, '') as PessoaSupervisor
                                    , convert(varchar,Alocacao.codigo) + ' | ' + Alocacao.descricao AS Alocacao
                                    , convert(varchar,TipoVinculo.codigo) + ' | ' + TipoVinculo.descricao AS TipoVinculo
                                    , func.IdHorarioDinamico
                                    , func.CicloSequenciaIndice
                                    , convert(varchar,HDN.codigo) +' | '+HDN.descricao HorarioDinamico
                             FROM funcionario func 
                             LEFT JOIN empresa ON empresa.id = func.idempresa
                             LEFT JOIN departamento ON departamento.id = func.iddepartamento
                             LEFT JOIN horario ON horario.id = func.idhorario
                             LEFT JOIN funcao ON funcao.id = func.idFuncao
                             LEFT JOIN cw_usuario cwu ON cwu.id = func.idcw_usuario
                             LEFT JOIN pessoa pe ON pe.id = func.IdPessoaSupervisor
                             LEFT JOIN Alocacao ON alocacao.id = func.idAlocacao
                             LEFT JOIN TipoVinculo ON TipoVinculo.id = func.idTipoVinculo
                             LEFT JOIN HorarioDinamico HDN ON HDN.id = func.idhorariodinamico
                             WHERE func.codigo = @codigo";
        }

        public string SqlLoadByCpfeMatricula()
        {
            return @"        SELECT   f.id,
                                       f.codigo,
                                       f.dscodigo,
                                       f.matricula,
                                       f.nome,
                                       f.codigofolha,
                                       f.idempresa,
                                       f.iddepartamento,
                                       f.idfuncao,
                                       f.idhorario,
                                       f.tipohorario,
                                       f.carteira,
                                       f.dataadmissao,
                                       f.datademissao,
                                       f.salario,
                                       f.funcionarioativo,
                                       f.DataInativacao,
                                       f.naoentrarbanco,
                                       f.naoentrarcompensacao,
                                       f.excluido,
                                       f.campoobservacao,
                                       f.incdata,
                                       f.inchora,
                                       f.incusuario,
                                       f.altdata,
                                       f.althora,
                                       f.altusuario,
                                       f.pis,
                                       f.senha,
                                       f.toleranciaentrada,
                                       f.toleranciasaida,
                                       f.quantidadetickets,
                                       f.tipotickets,
                                       f.CPF,
                                       f.Mob_Senha,
                                       f.idcw_usuario,
                                       f.utilizaregistrador,
                                       f.UtilizaAppPontofopag,
                                       f.UtilizaReconhecimentoFacialApp,
                                       f.UtilizaWebAppPontofopag,
                                       f.UtilizaReconhecimentoFacialWebApp,
                                       f.idIntegracao,
                                       f.IdPessoaSupervisor,
                                       f.TipoMaoObra,
                                       f.IdAlocacao,
                                       f.IdTipoVinculo,
                                       f.Email,
                                       f.IdIntegracaoPainel,
                                       f.RFID, 
                                        '' foto
                             FROM funcionario f 
                             WHERE CONVERT(BIGINT,REPLACE(REPLACE(f.cpf,'-',''),'.','')) = @cpf
							 and f.matricula = @matricula";
        }


        #region Metodos

        protected override bool SetInstance(SqlDataReader dr, Modelo.ModeloBase obj)
        {
            try
            {
                if (dr.Read())
                {
                    AuxSetInstance(dr, obj);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                obj = new Modelo.Funcionario();
                throw (ex);
            }
            finally
            {
                if (!dr.IsClosed)
                {
                    dr.Close();
                }
                dr.Dispose();
            }
        }

        private void AuxSetInstance(SqlDataReader dr, Modelo.ModeloBase obj)
        {
            SetInstanceBase(dr, obj);
            ((Modelo.Funcionario)obj).Codigo = Convert.ToInt32(dr["codigo"]);
            ((Modelo.Funcionario)obj).Dscodigo = Convert.ToString(dr["dscodigo"]);
            ((Modelo.Funcionario)obj).Matricula = Convert.ToString(dr["matricula"]);
            ((Modelo.Funcionario)obj).Nome = Convert.ToString(dr["nome"]);
            ((Modelo.Funcionario)obj).Codigofolha = (dr["codigofolha"] is DBNull ? Convert.ToInt32(0) : Convert.ToInt32(dr["codigofolha"]));
            ((Modelo.Funcionario)obj).Idempresa = Convert.ToInt32(dr["idempresa"]);
            ((Modelo.Funcionario)obj).Iddepartamento = Convert.ToInt32(dr["iddepartamento"]);
            ((Modelo.Funcionario)obj).Idfuncao = Convert.ToInt32(dr["idfuncao"]);
            ((Modelo.Funcionario)obj).Idhorario = (dr["idhorario"] is DBNull ? 0 : Convert.ToInt32(dr["idhorario"]));
            ((Modelo.Funcionario)obj).Tipohorario = Convert.ToInt16(dr["tipohorario"]);
            ((Modelo.Funcionario)obj).Carteira = Convert.ToString(dr["carteira"]);
            ((Modelo.Funcionario)obj).Contrato = Convert.ToString(dr["contrato"]);
            ((Modelo.Funcionario)obj).Dataadmissao = Convert.ToDateTime(dr["dataadmissao"]);
            ((Modelo.Funcionario)obj).Datademissao = (dr["datademissao"] is DBNull ? null : (DateTime?)(dr["datademissao"]));
            ((Modelo.Funcionario)obj).Salario = Convert.ToDecimal(dr["salario"]);
            ((Modelo.Funcionario)obj).Funcionarioativo = Convert.ToInt16(dr["funcionarioativo"]);
            ((Modelo.Funcionario)obj).Funcionarioativo_Ant = ((Modelo.Funcionario)obj).Funcionarioativo;
            ((Modelo.Funcionario)obj).Naoentrarbanco = Convert.ToInt16(dr["naoentrarbanco"]);
            ((Modelo.Funcionario)obj).Naoentrarbanco_Ant = ((Modelo.Funcionario)obj).Naoentrarbanco;
            ((Modelo.Funcionario)obj).Naoentrarcompensacao = Convert.ToInt16(dr["naoentrarcompensacao"]);
            ((Modelo.Funcionario)obj).Naoentrarcompensacao_Ant = ((Modelo.Funcionario)obj).Naoentrarcompensacao;
            ((Modelo.Funcionario)obj).Excluido = Convert.ToInt16(dr["excluido"]);
            ((Modelo.Funcionario)obj).Campoobservacao = Convert.ToString(dr["campoobservacao"]);
            ((Modelo.Funcionario)obj).Foto = Convert.ToString(dr["foto"]);
            ((Modelo.Funcionario)obj).Empresa = Convert.ToString(dr["empresa"]);
            ((Modelo.Funcionario)obj).Horario = Convert.ToString(dr["jornada"]);
            ((Modelo.Funcionario)obj).Pis = dr["Pis"] is DBNull ? String.Empty : Convert.ToString(dr["pis"]);
            ((Modelo.Funcionario)obj).Senha = dr["senha"] is DBNull ? String.Empty : Convert.ToString(dr["senha"]);
            ((Modelo.Funcionario)obj).ToleranciaEntrada = Convert.ToString(dr["toleranciaentrada"]) ?? null;
            ((Modelo.Funcionario)obj).ToleranciaSaida = Convert.ToString(dr["toleranciasaida"]) ?? null;
            ((Modelo.Funcionario)obj).QuantidadeTickets = dr["quantidadetickets"] is DBNull ? 0 : (int)dr["quantidadetickets"];
            ((Modelo.Funcionario)obj).TipoTickets = dr["tipotickets"] is DBNull ? 0 : (int)dr["tipotickets"];
            ((Modelo.Funcionario)obj).Iddepartamento_Ant = ((Modelo.Funcionario)obj).Iddepartamento;
            ((Modelo.Funcionario)obj).Idempresa_Ant = ((Modelo.Funcionario)obj).Idempresa;
            ((Modelo.Funcionario)obj).Idfuncao_Ant = ((Modelo.Funcionario)obj).Idfuncao;
            ((Modelo.Funcionario)obj).Dataadmissao_Ant = ((Modelo.Funcionario)obj).Dataadmissao;
            ((Modelo.Funcionario)obj).Datademissao_Ant = ((Modelo.Funcionario)obj).Datademissao;
            ((Modelo.Funcionario)obj).CPF = dr["CPF"] is DBNull ? String.Empty : Convert.ToString(dr["CPF"]);
            ((Modelo.Funcionario)obj).Mob_Senha = dr["Mob_Senha"] is DBNull ? String.Empty : Convert.ToString(dr["Mob_Senha"]);
            ((Modelo.Funcionario)obj).Jornada = dr["jornada"] is DBNull ? String.Empty : Convert.ToString(dr["jornada"]);
            ((Modelo.Funcionario)obj).Departamento = dr["departamento"] is DBNull ? String.Empty : Convert.ToString(dr["departamento"]);
            ((Modelo.Funcionario)obj).Funcao = dr["funcao"] is DBNull ? String.Empty : Convert.ToString(dr["funcao"]);
            ((Modelo.Funcionario)obj).TipoMaoObra = dr["TipoMaoObra"] is DBNull ? null : (int?)(dr["TipoMaoObra"]);
            ((Modelo.Funcionario)obj).IdAlocacao = dr["IdAlocacao"] is DBNull ? null : (int?)(dr["IdAlocacao"]);
            ((Modelo.Funcionario)obj).Alocacao = dr["Alocacao"] is DBNull ? String.Empty : Convert.ToString(dr["Alocacao"]);
            ((Modelo.Funcionario)obj).IdTipoVinculo = dr["IdTipoVinculo"] is DBNull ? null : (int?)(dr["IdTipoVinculo"]);
            ((Modelo.Funcionario)obj).TipoVinculo = dr["TipoVinculo"] is DBNull ? String.Empty : Convert.ToString(dr["TipoVinculo"]);
            ((Modelo.Funcionario)obj).IdIntegracaoPainel = dr["IdIntegracaoPainel"] is DBNull ? null : (int?)(dr["IdIntegracaoPainel"]);
            ((Modelo.Funcionario)obj).Email = Convert.ToString(dr["Email"]);
            if (dr["idcw_usuario"] is DBNull)
            {
                ((Modelo.Funcionario)obj).IdCw_Usuario = null;
            }
            else
            {
                ((Modelo.Funcionario)obj).IdCw_Usuario = Convert.ToInt32(dr["idcw_usuario"]);
            }
            ((Modelo.Funcionario)obj).Supervisor = dr["Supervisor"] is DBNull ? String.Empty : Convert.ToString(dr["Supervisor"]);
            ((Modelo.Funcionario)obj).utilizaregistrador = dr["utilizaregistrador"] is DBNull ? false : Convert.ToBoolean(dr["utilizaregistrador"]);
            ((Modelo.Funcionario)obj).UtilizaAppPontofopag = dr["UtilizaAppPontofopag"] is DBNull ? false : Convert.ToBoolean(dr["UtilizaAppPontofopag"]);
            ((Modelo.Funcionario)obj).UtilizaReconhecimentoFacialApp = dr["UtilizaReconhecimentoFacialApp"] is DBNull ? false : Convert.ToBoolean(dr["UtilizaReconhecimentoFacialApp"]);
            ((Modelo.Funcionario)obj).UtilizaWebAppPontofopag = dr["UtilizaWebAppPontofopag"] is DBNull ? false : Convert.ToBoolean(dr["UtilizaWebAppPontofopag"]);
            ((Modelo.Funcionario)obj).UtilizaReconhecimentoFacialWebApp = dr["UtilizaReconhecimentoFacialWebApp"] is DBNull ? false : Convert.ToBoolean(dr["UtilizaReconhecimentoFacialWebApp"]);

            object val = dr["idIntegracao"];
            Int32? idint = (val == null || val is DBNull) ? (Int32?)null : (Int32?)val;
            ((Modelo.Funcionario)obj).idIntegracao = idint;
            ((Modelo.Funcionario)obj).PessoaSupervisor = dr["PessoaSupervisor"] is DBNull ? String.Empty : Convert.ToString(dr["PessoaSupervisor"]);
            if (dr["IdPessoaSupervisor"] is DBNull)
            {
                ((Modelo.Funcionario)obj).IdPessoaSupervisor = null;
            }
            else
            {
                ((Modelo.Funcionario)obj).IdPessoaSupervisor = Convert.ToInt32(dr["IdPessoaSupervisor"]);
            }

            ((Modelo.Funcionario)obj).RFID = dr["RFID"] is DBNull ? (Int64?)null : Convert.ToInt64(dr["RFID"]);
            ((Modelo.Funcionario)obj).RFID_Ant = ((Modelo.Funcionario)obj).RFID;
            ((Modelo.Funcionario)obj).IdHorarioDinamico = dr["IdHorarioDinamico"] is DBNull ? (Int32?)null : Convert.ToInt32(dr["IdHorarioDinamico"]);
            ((Modelo.Funcionario)obj).CicloSequenciaIndice = dr["CicloSequenciaIndice"] is DBNull ? (Int32?)null : Convert.ToInt32(dr["CicloSequenciaIndice"]);
            ((Modelo.Funcionario)obj).HorarioDinamico = dr["HorarioDinamico"] is DBNull ? String.Empty : Convert.ToString(dr["HorarioDinamico"]);
            ((Modelo.Funcionario)obj).DataInativacao = (dr["DataInativacao"] is DBNull ? null : (DateTime?)(dr["DataInativacao"]));
            ((Modelo.Funcionario)obj).DataInativacao_Ant = ((Modelo.Funcionario)obj).DataInativacao;
        }

        protected override SqlParameter[] GetParameters()
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter ("@id", SqlDbType.Int),
                new SqlParameter ("@codigo", SqlDbType.Int),
                new SqlParameter ("@dscodigo", SqlDbType.VarChar),
                new SqlParameter ("@matricula", SqlDbType.VarChar),
                new SqlParameter ("@nome", SqlDbType.VarChar),
                new SqlParameter ("@codigofolha", SqlDbType.Int),
                new SqlParameter ("@idempresa", SqlDbType.Int),
                new SqlParameter ("@iddepartamento", SqlDbType.Int),
                new SqlParameter ("@idfuncao", SqlDbType.Int),
                new SqlParameter ("@idhorario", SqlDbType.Int),
                new SqlParameter ("@tipohorario", SqlDbType.TinyInt),
                new SqlParameter ("@carteira", SqlDbType.VarChar),
                new SqlParameter ("@dataadmissao", SqlDbType.DateTime),
                new SqlParameter ("@datademissao", SqlDbType.DateTime),
                new SqlParameter ("@salario", SqlDbType.Decimal),
                new SqlParameter ("@naoentrarbanco", SqlDbType.TinyInt),
                new SqlParameter ("@naoentrarcompensacao", SqlDbType.TinyInt),
                new SqlParameter ("@excluido", SqlDbType.TinyInt),
                new SqlParameter ("@campoobservacao", SqlDbType.VarChar),
                new SqlParameter ("@foto", SqlDbType.VarChar),
                new SqlParameter ("@incdata", SqlDbType.DateTime),
                new SqlParameter ("@inchora", SqlDbType.DateTime),
                new SqlParameter ("@incusuario", SqlDbType.VarChar),
                new SqlParameter ("@altdata", SqlDbType.DateTime),
                new SqlParameter ("@althora", SqlDbType.DateTime),
                new SqlParameter ("@altusuario", SqlDbType.VarChar),
                new SqlParameter ("@pis", SqlDbType.VarChar),
                new SqlParameter ("@senha", SqlDbType.VarChar),
                new SqlParameter ("@toleranciaentrada", SqlDbType.VarChar),
                new SqlParameter ("@toleranciasaida", SqlDbType.VarChar),
                new SqlParameter ("@quantidadetickets", SqlDbType.Int),
                new SqlParameter ("@tipotickets", SqlDbType.Int),
                new SqlParameter ("@CPF", SqlDbType.VarChar),
                new SqlParameter ("@Mob_Senha", SqlDbType.VarChar),
                new SqlParameter ("@idcw_usuario", SqlDbType.Int),
                new SqlParameter ("@utilizaregistrador", SqlDbType.Int),
                new SqlParameter ("@IdIntegracao", SqlDbType.Int),
                new SqlParameter ("@IdPessoaSupervisor", SqlDbType.Int),
                new SqlParameter ("@TipoMaoObra", SqlDbType.Int),
                new SqlParameter ("@IdAlocacao", SqlDbType.Int),
                new SqlParameter ("@IdTipoVinculo", SqlDbType.Int),
                new SqlParameter ("@IdIntegracaoPainel", SqlDbType.Int),
                new SqlParameter ("@Email", SqlDbType.VarChar),
                new SqlParameter ("@RFID", SqlDbType.BigInt),
                new SqlParameter ("@IdHorarioDinamico", SqlDbType.Int),
                new SqlParameter ("@CicloSequenciaIndice", SqlDbType.Int),
                new SqlParameter ("@DataInativacao", SqlDbType.DateTime),
                new SqlParameter ("@UtilizaAppPontofopag", SqlDbType.Bit),
                new SqlParameter ("@UtilizaReconhecimentoFacialApp", SqlDbType.Bit),
                new SqlParameter ("@UtilizaWebAppPontofopag", SqlDbType.Bit),
                new SqlParameter ("@UtilizaReconhecimentoFacialWebApp", SqlDbType.Bit)
            };
            return parms;
        }

        protected override void SetParameters(SqlParameter[] parms, Modelo.ModeloBase obj)
        {
            parms[0].Value = obj.Id;
            if (obj.Id == 0)
            {
                parms[0].Direction = ParameterDirection.Output;
            }
            parms[1].Value = ((Modelo.Funcionario)obj).Codigo;
            parms[2].Value = ((Modelo.Funcionario)obj).Dscodigo;
            parms[3].Value = ((Modelo.Funcionario)obj).Matricula;
            parms[4].Value = ((Modelo.Funcionario)obj).Nome;
            parms[5].Value = ((Modelo.Funcionario)obj).Codigofolha;
            parms[6].Value = ((Modelo.Funcionario)obj).Idempresa;
            parms[7].Value = ((Modelo.Funcionario)obj).Iddepartamento;
            parms[8].Value = ((Modelo.Funcionario)obj).Idfuncao;
            if (((Modelo.Funcionario)obj).Idhorario > 0)
                parms[9].Value = ((Modelo.Funcionario)obj).Idhorario;
            parms[10].Value = ((Modelo.Funcionario)obj).Tipohorario;
            parms[11].Value = ((Modelo.Funcionario)obj).Carteira;
            parms[12].Value = ((Modelo.Funcionario)obj).Dataadmissao;

            if (((Modelo.Funcionario)obj).Datademissao != new DateTime())
                parms[13].Value = ((Modelo.Funcionario)obj).Datademissao;
            else
                parms[13].Value = DBNull.Value;

            parms[14].Value = ((Modelo.Funcionario)obj).Salario;
            parms[15].Value = ((Modelo.Funcionario)obj).Naoentrarbanco;
            parms[16].Value = ((Modelo.Funcionario)obj).Naoentrarcompensacao;
            parms[17].Value = ((Modelo.Funcionario)obj).Excluido;
            parms[18].Value = ((Modelo.Funcionario)obj).Campoobservacao;
            parms[19].Value = ((Modelo.Funcionario)obj).Foto;
            parms[20].Value = ((Modelo.Funcionario)obj).Incdata;
            parms[21].Value = ((Modelo.Funcionario)obj).Inchora;
            parms[22].Value = ((Modelo.Funcionario)obj).Incusuario;
            parms[23].Value = ((Modelo.Funcionario)obj).Altdata;
            parms[24].Value = ((Modelo.Funcionario)obj).Althora;
            parms[25].Value = ((Modelo.Funcionario)obj).Altusuario;
            parms[26].Value = ((Modelo.Funcionario)obj).Pis;
            parms[27].Value = ((Modelo.Funcionario)obj).Senha;
            parms[28].Value = ((Modelo.Funcionario)obj).ToleranciaEntrada;
            parms[29].Value = ((Modelo.Funcionario)obj).ToleranciaSaida;
            parms[30].Value = ((Modelo.Funcionario)obj).QuantidadeTickets;
            parms[31].Value = ((Modelo.Funcionario)obj).TipoTickets;
            parms[32].Value = ((Modelo.Funcionario)obj).CPF;
            parms[33].Value = ((Modelo.Funcionario)obj).Mob_Senha;
            parms[34].Value = ((Modelo.Funcionario)obj).IdCw_Usuario;
            parms[35].Value = ((Modelo.Funcionario)obj).utilizaregistrador;
            parms[36].Value = ((Modelo.Funcionario)obj).idIntegracao;
            parms[37].Value = ((Modelo.Funcionario)obj).IdPessoaSupervisor;
            parms[38].Value = ((Modelo.Funcionario)obj).TipoMaoObra;
            parms[39].Value = ((Modelo.Funcionario)obj).IdAlocacao;
            parms[40].Value = ((Modelo.Funcionario)obj).IdTipoVinculo == 0 ? null : ((Modelo.Funcionario)obj).IdTipoVinculo;
            parms[41].Value = ((Modelo.Funcionario)obj).IdIntegracaoPainel == 0 ? null : ((Modelo.Funcionario)obj).IdIntegracaoPainel;
            parms[42].Value = ((Modelo.Funcionario)obj).Email;
            parms[43].Value = ((Modelo.Funcionario)obj).RFID == 0 ? null : ((Modelo.Funcionario)obj).RFID;
            parms[44].Value = ((Modelo.Funcionario)obj).IdHorarioDinamico == 0 ? null : ((Modelo.Funcionario)obj).IdHorarioDinamico;
            parms[45].Value = ((Modelo.Funcionario)obj).CicloSequenciaIndice == 0 ? null : ((Modelo.Funcionario)obj).CicloSequenciaIndice;
            if (((Modelo.Funcionario)obj).DataInativacao != null && ((Modelo.Funcionario)obj).DataInativacao != new DateTime())
                parms[46].Value = (((Modelo.Funcionario)obj).DataInativacao).Value.Date;
            else
                parms[46].Value = DBNull.Value;
            parms[47].Value = ((Modelo.Funcionario)obj).UtilizaAppPontofopag;
            parms[48].Value = ((Modelo.Funcionario)obj).UtilizaReconhecimentoFacialApp;
            parms[49].Value = ((Modelo.Funcionario)obj).UtilizaWebAppPontofopag;
            parms[50].Value = ((Modelo.Funcionario)obj).UtilizaReconhecimentoFacialWebApp;
        }

        public bool VerificaCodigoDuplicado(string pCodigo)
        {
            bool ret = false;
            SqlParameter[] parms = new SqlParameter[1]
            {
                  new SqlParameter("@dscodigo", SqlDbType.VarChar)
            };
            parms[0].Value = pCodigo;

            string aux = @"SELECT ISNULL(COUNT(id),0) AS qtd FROM funcionario WHERE dscodigo = @dscodigo";

            SqlDataReader dr;
            dr = db.ExecuteReader(CommandType.Text, aux, parms);
            if (dr.Read())
            {
                if (Convert.ToInt32(dr["Qtd"]) > 0)
                    ret = true;
                else
                    ret = false;
            }
            else
                ret = false;

            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return ret;
        }

        public Modelo.Funcionario LoadObject(int id)
        {
            SqlParameter[] parms = new SqlParameter[] { new SqlParameter("@id", SqlDbType.Int, 4) };
            parms[0].Value = id;
            string sql = SELECTPID + PermissaoUsuarioFuncionario(UsuarioLogado, SELECTPID, "func.idempresa", "func.id", null);
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

            Modelo.Funcionario objFuncionario = new Modelo.Funcionario();
            try
            {
                SetInstance(dr, objFuncionario);
                objFuncionario.Historico = dalFuncionarioHistorico.LoadPorFuncionario(objFuncionario.Id);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (!dr.IsClosed)
                {
                    dr.Close();
                }
                dr.Dispose();
            }
            return objFuncionario;
        }

        public Modelo.Funcionario LoadPorCPF(string CPF)
        {
            SqlDataReader dr = LoadDataReaderCPF(CPF);

            Modelo.Funcionario objFuncionario = new Modelo.Funcionario();
            try
            {
                SetInstance(dr, objFuncionario);
                objFuncionario.Historico = dalFuncionarioHistorico.LoadPorFuncionario(objFuncionario.Id);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (!dr.IsClosed)
                {
                    dr.Close();
                }
                dr.Dispose();
            }
            return objFuncionario;
        }

        public Modelo.Funcionario LoadAtivoPorCPF(string CPF)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@CPF", SqlDbType.VarChar)
            };
            parms[0].Value = CPF;

            Modelo.Funcionario objFuncionario = new Modelo.Funcionario();

            string sql = SELECTPCPF + " AND ISNULL(func.funcionarioativo, 0) = 1 ";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);
            try
            {
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        objFuncionario = new Modelo.Funcionario();
                        AuxSetInstance(dr, objFuncionario);

                        objFuncionario.Historico = dalFuncionarioHistorico.LoadPorFuncionario(objFuncionario.Id);
                        objFuncionario.Biometrias = dalBiometrias.LoadPorFuncionario(objFuncionario.Id);
                    }
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (!dr.IsClosed)
                {
                    dr.Close();
                }
                dr.Dispose();
            }
            return objFuncionario;
        }

        public List<Modelo.Funcionario> LoadAtivoPorListCPF(List<string> CPFs)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    List<Modelo.Funcionario> objListaFuncionario = new List<Modelo.Funcionario>();

                    string sql = @"SELECT  func.id,
                                           func.codigo,
                                           func.dscodigo,
                                           func.matricula,
                                           func.nome,
                                           func.codigofolha,
                                           func.idempresa,
                                           func.iddepartamento,
                                           func.idfuncao,
                                           func.idhorario,
                                           func.tipohorario,
                                           func.carteira,
                                           func.dataadmissao,
                                           func.datademissao,
                                           func.salario,
                                           func.funcionarioativo,
                                           func.DataInativacao,
                                           func.naoentrarbanco,
                                           func.naoentrarcompensacao,
                                           func.excluido,
                                           func.campoobservacao,
                                           func.incdata,
                                           func.inchora,
                                           func.incusuario,
                                           func.altdata,
                                           func.althora,
                                           func.altusuario,
                                           func.pis,
                                           func.senha,
                                           func.toleranciaentrada,
                                           func.toleranciasaida,
                                           func.quantidadetickets,
                                           func.tipotickets,
                                           func.CPF,
                                           func.Mob_Senha,
                                           func.idcw_usuario,
                                           func.utilizaregistrador,
                                           func.UtilizaAppPontofopag,
                                           func.UtilizaReconhecimentoFacialApp,
                                           func.UtilizaWebAppPontofopag,
                                           func.UtilizaReconhecimentoFacialWebApp,
                                           func.idIntegracao,
                                           func.IdPessoaSupervisor,
                                           func.TipoMaoObra,
                                           func.IdAlocacao,
                                           func.IdTipoVinculo,
                                           func.Email,
                                           func.IdIntegracaoPainel,
                                           func.RFID,
                                           '' foto,
                                            '' contrato,
                                horario.descricao AS jornada ,
                                CONVERT(VARCHAR, empresa.codigo) + ' | ' + empresa.nome AS empresa ,
                                CONVERT(VARCHAR, departamento.codigo) + ' | ' + departamento.descricao AS departamento ,
                                CONVERT(VARCHAR, funcao.codigo) + ' | ' + funcao.descricao AS funcao ,
                                COALESCE(CONVERT(VARCHAR, cwu.codigo) + ' | ' + cwu.nome, '') AS supervisor ,
                                COALESCE(CONVERT(VARCHAR, pe.codigo) + ' | ' + pe.RazaoSocial, '') AS PessoaSupervisor ,
                                CONVERT(VARCHAR, Alocacao.codigo) + ' | ' + Alocacao.descricao AS Alocacao ,
                                CONVERT(VARCHAR, TipoVinculo.codigo) + ' | ' + TipoVinculo.descricao AS TipoVinculo,
                                func.IdHorarioDinamico, 
                                func.CicloSequenciaIndice,
                                convert(varchar,HDN.codigo) +' | '+HDN.descricao HorarioDinamico
                            FROM    funcionario func
                                    LEFT JOIN empresa ON empresa.id = func.idempresa
                                    LEFT JOIN departamento ON departamento.id = func.iddepartamento
                                    LEFT JOIN horario ON horario.id = func.idhorario
                                    LEFT JOIN funcao ON funcao.id = func.idfuncao
                                    LEFT JOIN cw_usuario cwu ON cwu.id = func.idcw_usuario
                                    LEFT JOIN Pessoa pe ON pe.id = func.IdPessoaSupervisor
                                    LEFT JOIN Alocacao ON Alocacao.id = func.IdAlocacao
                                    LEFT JOIN TipoVinculo ON TipoVinculo.id = func.IdTipoVinculo
                                    LEFT JOIN HorarioDinamico HDN ON HDN.id = func.idhorariodinamico
                            WHERE   func.CPF IN ({CPF})  
                            AND ISNULL(func.funcionarioativo, 0) = 1;";

                    cmd.Connection = db.GetConnection;
                    cmd.CommandText = sql;
                    cmd.CommandTimeout = 600000;


                    db.AddArrayParameters(cmd, CPFs, "CPF");
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var objFuncionario = new Modelo.Funcionario();
                            AuxSetInstance(dr, objFuncionario);

                            objFuncionario.Historico = dalFuncionarioHistorico.LoadPorFuncionario(objFuncionario.Id);
                            objFuncionario.Biometrias = dalBiometrias.LoadPorFuncionario(objFuncionario.Id);
                            objListaFuncionario.Add(objFuncionario);
                        }
                    }
                    return objListaFuncionario;
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        #region Relatórios

        public DataTable GetOrdenadoPorNomeRel(string pInicial, string pFinal, string pEmpresas)
        {
            SqlParameter[] parms = new SqlParameter[0];
            DataTable dt = new DataTable();
            string aux = SELECTREL + " AND func.idempresa IN " + pEmpresas + " AND ISNULL(func.funcionarioativo, 0) = 1 ";
            if (!String.IsNullOrEmpty(pInicial))
            {
                aux += " AND UPPER (func.nome) >= '" + pInicial.ToUpper() + "'";
            }
            if (!String.IsNullOrEmpty(pFinal))
            {
                aux += " AND SUBSTRING (UPPER (func.nome), 1, " + pFinal.Length + ") <= '" + pFinal.ToUpper() + "'";
            }
            aux += " ORDER BY func.nome";
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            dt.Load(dr);
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return dt;
        }

        public DataTable GetOrdenadoPorNomeRel(List<int> idsFuncs)
        {
            SqlParameter[] parms = new SqlParameter[1]
            {
                new SqlParameter("@idsFuncs", SqlDbType.VarChar)
            };

            parms[0].Value = String.Join(",", idsFuncs);

            DataTable dt = new DataTable();
            string aux = SELECTREL + " AND func.id IN (SELECT * FROM dbo.F_ClausulaIn(@idsFuncs)) ";
            aux += " ORDER BY func.nome";
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            dt.Load(dr);
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return dt;
        }

        public DataTable GetOrdenadoPorCodigoRel(string pInicial, string pFinal, string pEmpresas)
        {
            SqlParameter[] parms = new SqlParameter[2]
            {
                  new SqlParameter("@Inicial", SqlDbType.VarChar)
                , new SqlParameter("@Final", SqlDbType.VarChar)
            };
            parms[0].Value = pInicial;
            parms[1].Value = pFinal;
            DataTable dt = new DataTable();
            string aux = SELECTREL + " AND func.idempresa IN " + pEmpresas + " AND ISNULL(func.funcionarioativo, 0) = 1 ";
            if ((pInicial != "0") || (pFinal != "0"))
            {
                aux = aux + @" AND ((CAST(func.dscodigo AS BIGINT) >= @Inicial) AND (CAST(func.dscodigo AS BIGINT) <= @Final)) ORDER BY codigo";
            }
            else
            {
                aux = aux + " ORDER BY CAST(func.dscodigo AS BIGINT), func.nome";
            }
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            dt.Load(dr);
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return dt;
        }

        public DataTable GetOrdenadoPorCodigoRel(List<int> idsFuncs)
        {
            SqlParameter[] parms = new SqlParameter[1]
            {
                  new SqlParameter("@idsFuncs", SqlDbType.VarChar)
            };
            parms[0].Value = String.Join(",", idsFuncs);
            DataTable dt = new DataTable();
            string aux = SELECTREL + " AND func.id IN (SELECT * FROM dbo.F_ClausulaIn(@idsFuncs)) AND ISNULL(func.funcionarioativo, 0) = 1 ";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            dt.Load(dr);
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return dt;
        }

        public DataTable GetRelatorio(string pEmpresas)
        {
            SqlParameter[] parms = new SqlParameter[0];
            DataTable dt = new DataTable();
            string aux = SELECTREL + " AND func.idempresa IN " + pEmpresas + " AND ISNULL(func.funcionarioativo, 0) = 1 ";
            aux += " ORDER BY func.nome";
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            dt.Load(dr);
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return dt;
        }

        public DataTable GetRelatorio(List<int> idsFuncs)
        {
            SqlParameter[] parms = new SqlParameter[1]
            {
                  new SqlParameter("@idsFuncs", SqlDbType.VarChar)
            };
            parms[0].Value = String.Join(",", idsFuncs);
            DataTable dt = new DataTable();
            string aux = SELECTREL + " AND func.id IN (SELECT * FROM dbo.F_ClausulaIn(@idsFuncs)) AND ISNULL(func.funcionarioativo, 0) = 1 ";
            aux += " ORDER BY func.nome";
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            dt.Load(dr);
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return dt;
        }

        public DataTable GetPorDepartamentoRel(string pDepartamentos)
        {
            SqlParameter[] parms = new SqlParameter[0];

            DataTable dt = new DataTable();

            string aux;

            aux = @SELECTREL + " AND func.IdDepartamento IN " + pDepartamentos + " AND ISNULL(func.funcionarioativo, 0) = 1 ";
            aux += " ORDER BY departamento, func.nome";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            dt.Load(dr);
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return dt;
        }

        public DataTable GetPorDepartamentoRel(List<int> idsFuncs)
        {
            SqlParameter[] parms = new SqlParameter[1]
            {
                  new SqlParameter("@idsFuncs", SqlDbType.VarChar)
            };
            parms[0].Value = String.Join(",", idsFuncs);
            DataTable dt = new DataTable();

            string aux;

            aux = @SELECTREL + " AND func.id IN (SELECT * FROM dbo.F_ClausulaIn(@idsFuncs)) AND ISNULL(func.funcionarioativo, 0) = 1 ";
            aux += " ORDER BY departamento, func.nome";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            dt.Load(dr);
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return dt;
        }


        public DataTable GetPorDepartamento(string pDepartamentos)
        {
            SqlParameter[] parms = new SqlParameter[0];

            DataTable dt = new DataTable();

            string aux;

            aux = @"   SELECT   func.id
                                    , CAST(func.dscodigo AS BIGINT) AS codigo 
                                    , func.nome
                                    , func.funcionarioativo
                                    , func.matricula AS matricula
                                    , ISNULL (func.dataadmissao,' ') AS dataadmissao
                                    , ISNULL (func.datademissao,' ') AS datademissao
                                    , ISNULL (func.TipoMaoObra, ' ') AS TipoMaoObra
                                    , convert(varchar,dep.codigo)+' | '+dep.descricao AS departamento
                                    , convert(varchar,emp.codigo)+' | '+emp.nome as empresa
                                    , convert(varchar,funcao.codigo) + ' | ' + funcao.descricao AS funcao
                                    , coalesce(convert(varchar,cwu.codigo) + ' | ' + cwu.nome, '') AS supervisor
                                    , coalesce(convert(varchar, pe.codigo) + ' | ' + pe.razaosocial, '') as PessoaSupervisor
                                    , convert(varchar,Alocacao.codigo) + ' | ' + Alocacao.descricao AS Alocacao
                                    , convert(varchar,TipoVinculo.codigo) + ' | ' + TipoVinculo.descricao AS TipoVinculo
                                    , func.IdAlocacao
                                    , func.IdTipoVinculo
                                    , hor.descricao AS horario
                                    , (SELECT TOP 1 ISNULL(hd.entrada_1, '--:--') + ' - ' + ISNULL(hd.saida_1, '--:--') + ' | ' + ISNULL(hd.entrada_2, '--:--') + ' - ' + ISNULL(hd.saida_2, '--:--') 
                                    + ' | ' + ISNULL(hd.entrada_3, '--:--') + ' - ' + ISNULL(hd.saida_3, '--:--') + ' | ' + ISNULL(hd.entrada_4, '--:--') + ' - ' + ISNULL(hd.saida_4, '--:--')
                                      FROM horariodetalhe hd WHERE hd.idhorario = hor.id AND (hd.dia = 1 OR DATEPART(WEEKDAY, hd.data) = 2)) AS horariodescricao
                             FROM funcionario func
                             LEFT JOIN empresa emp ON emp.id = func.idempresa
                             LEFT JOIN departamento dep ON dep.id = func.iddepartamento
                             LEFT JOIN horario hor ON hor.id = func.idhorario
                             LEFT JOIN funcao ON funcao.id = func.idFuncao
                             LEFT JOIN cw_usuario cwu ON cwu.id = func.idcw_usuario
                             LEFT JOIN pessoa pe ON pe.id = func.IdPessoaSupervisor
                             LEFT JOIN Alocacao ON alocacao.id = func.idAlocacao
                             LEFT JOIN TipoVinculo ON TipoVinculo.id = func.IdTipoVinculo
                             WHERE ISNULL(func.excluido, 0) = 0 AND func.IdDepartamento IN " + pDepartamentos;
            aux += " ORDER BY departamento, func.nome";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            dt.Load(dr);
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return dt;
        }

        public DataTable GetPorHorarioRel(string pHorarios, string pEmpresas)
        {
            SqlParameter[] parms = new SqlParameter[0];

            DataTable dt = new DataTable();

            string aux;

            aux = @SELECTREL + " AND func.Idhorario IN " + pHorarios;
            aux += " AND func.Idempresa IN " + pEmpresas + " AND ISNULL(func.funcionarioativo, 0) = 1 ";
            aux += " ORDER BY horario, func.nome";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            dt.Load(dr);
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return dt;
        }

        public DataTable GetPorHorarioRel(List<int> idsFuncs)
        {
            SqlParameter[] parms = new SqlParameter[1]
            {
                  new SqlParameter("@idsFuncs", SqlDbType.VarChar)
            };
            parms[0].Value = String.Join(",", idsFuncs);
            DataTable dt = new DataTable();

            string aux;

            aux = @SELECTREL + " AND func.id IN (SELECT * FROM dbo.F_ClausulaIn(@idsFuncs)) ";
            aux += " AND ISNULL(func.funcionarioativo, 0) = 1 ";
            aux += " ORDER BY horario, func.nome";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            dt.Load(dr);
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return dt;
        }

        public DataTable GetPorDataAdmissaoRel(DateTime? pInicial, DateTime? pFinal, string pEmpresas)
        {
            SqlParameter[] parms = new SqlParameter[2]
            {
                  new SqlParameter("@Inicial", SqlDbType.DateTime)
                , new SqlParameter("@Final", SqlDbType.DateTime)
            };
            parms[0].Value = pInicial;
            parms[1].Value = pFinal;
            DataTable dt = new DataTable();
            string aux = SELECTREL + " AND func.idempresa IN " + pEmpresas;
            aux += " AND ISNULL(func.funcionarioativo, 0) = 1 ";

            if (pInicial != new DateTime() && pFinal != new DateTime())
            {
                parms[0].Value = pInicial;
                parms[1].Value = pFinal;
                aux = aux + @" AND ((func.dataadmissao >= @Inicial) AND (func.dataadmissao <= @Final))";
            }
            else if (pInicial != new DateTime())
            {
                parms[0].Value = pInicial;
                aux = aux + @" AND ((func.dataadmissao >= @Inicial)";
            }
            else if (pFinal != new DateTime())
            {
                parms[1].Value = pFinal;
                aux = aux + @" AND ((func.dataadmissao <= @Final)";
            }
            aux = aux + " ORDER BY func.dataadmissao, codigo";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            dt.Load(dr);
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return dt;
        }

        public DataTable GetPorDataAdmissaoRel(DateTime? pInicial, DateTime? pFinal, List<int> idsFuncs)
        {
            SqlParameter[] parms = new SqlParameter[3]
            {
                  new SqlParameter("@Inicial", SqlDbType.DateTime)
                , new SqlParameter("@Final", SqlDbType.DateTime)
                , new SqlParameter("@idsFuncs", SqlDbType.VarChar)
            };
            parms[0].Value = pInicial;
            parms[1].Value = pFinal;
            parms[2].Value = String.Join(",", idsFuncs);
            DataTable dt = new DataTable();
            string aux = SELECTREL + " AND func.id IN (SELECT * FROM dbo.F_ClausulaIn(@idsFuncs)) ";
            aux += " AND ISNULL(func.funcionarioativo, 0) = 1 ";

            if (pInicial != new DateTime() && pFinal != new DateTime())
            {
                parms[0].Value = pInicial;
                parms[1].Value = pFinal;
                aux = aux + @" AND ((func.dataadmissao >= @Inicial) AND (func.dataadmissao <= @Final))";
            }
            else if (pInicial != new DateTime())
            {
                parms[0].Value = pInicial;
                aux = aux + @" AND ((func.dataadmissao >= @Inicial)";
            }
            else if (pFinal != new DateTime())
            {
                parms[1].Value = pFinal;
                aux = aux + @" AND ((func.dataadmissao <= @Final)";
            }
            aux = aux + " ORDER BY func.dataadmissao, codigo";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            dt.Load(dr);
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return dt;
        }

        public DataTable GetPorDataDemissaoRel(DateTime? pInicial, DateTime? pFinal, string pEmpresas)
        {
            SqlParameter[] parms = new SqlParameter[2]
            {
                  new SqlParameter("@Inicial", SqlDbType.DateTime)
                , new SqlParameter("@Final", SqlDbType.DateTime)
            };

            parms[0].Value = pInicial;
            parms[1].Value = pFinal;

            DataTable dt = new DataTable();
            string aux = SELECTREL + " AND func.idempresa IN " + pEmpresas;
            aux += " AND ISNULL(func.funcionarioativo, 0) = 0 AND func.datademissao IS NOT NULL ";

            if (pInicial != new DateTime() && pFinal != new DateTime())
                aux = aux + @" AND ((func.datademissao >= @Inicial) AND (func.datademissao <= @Final))";

            else if (pInicial != new DateTime())
                aux = aux + @" AND (func.datademissao >= @Inicial)";

            else if (pFinal != new DateTime())
                aux = aux + @" AND (func.datademissao <= @Final)";

            aux = aux + " ORDER BY func.datademissao, codigo";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            dt.Load(dr);
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return dt;
        }

        public DataTable GetPorDataDemissaoRel(DateTime? pInicial, DateTime? pFinal, List<int> idsFuncs)
        {
            SqlParameter[] parms = new SqlParameter[3]
            {
                  new SqlParameter("@Inicial", SqlDbType.DateTime)
                , new SqlParameter("@Final", SqlDbType.DateTime)
                , new SqlParameter("@idsFuncs", SqlDbType.VarChar)
            };

            parms[0].Value = pInicial;
            parms[1].Value = pFinal;
            parms[2].Value = String.Join(",", idsFuncs);

            DataTable dt = new DataTable();
            string aux = SELECTREL + " AND func.id IN (SELECT * FROM dbo.F_ClausulaIn(@idsFuncs)) ";
            aux += " AND ISNULL(func.funcionarioativo, 0) = 0 AND func.datademissao IS NOT NULL ";

            if (pInicial != new DateTime() && pFinal != new DateTime())
                aux = aux + @" AND ((func.datademissao >= @Inicial) AND (func.datademissao <= @Final))";

            else if (pInicial != new DateTime())
                aux = aux + @" AND (func.datademissao >= @Inicial)";

            else if (pFinal != new DateTime())
                aux = aux + @" AND (func.datademissao <= @Final)";

            aux = aux + " ORDER BY func.datademissao, codigo";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            dt.Load(dr);
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return dt;
        }

        public DataTable GetAtivosInativosRel(bool pAtivo, string pEmpresas)
        {
            SqlParameter[] parms = new SqlParameter[0];
            DataTable dt = new DataTable();

            string aux = SELECTREL + " AND func.idempresa IN " + pEmpresas + " AND ISNULL(func.funcionarioativo, 0) = ";
            aux += (pAtivo ? "1" : "0");
            aux += " ORDER BY func.nome, codigo";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            dt.Load(dr);
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return dt;
        }

        public DataTable GetAtivosInativosRel(bool pAtivo, List<int> idsFuncs)
        {
            SqlParameter[] parms = new SqlParameter[1]
            {
                  new SqlParameter("@idsFuncs", SqlDbType.VarChar)
            };
            parms[0].Value = String.Join(",", idsFuncs);
            DataTable dt = new DataTable();

            string aux = SELECTREL + " AND func.id IN (SELECT * FROM dbo.F_ClausulaIn(@idsFuncs)) AND ISNULL(func.funcionarioativo, 0) = ";
            aux += (pAtivo ? "1" : "0");
            aux += " ORDER BY func.nome, codigo";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            dt.Load(dr);
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return dt;
        }

        public DataTable GetListaPresenca(DateTime dataInicial, int tipo, string empresas, string departamentos, string funcionarios)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@datainicial", SqlDbType.DateTime)
            };
			parms[0].Value = dataInicial.Date;

            DataTable dt = new DataTable();
            string aux = @"SELECT   empresa.nome as empresa
									, empresa.endereco
		                            , empresa.cnpj
		                            , funcionario.nome
		                            , funcionario.dscodigo
		                            , departamento.descricao AS departamento
		                            , marcacao.entrada_1
		                            , marcacao.saida_1
		                            , marcacao.entrada_2
		                            , marcacao.saida_2
		                            , marcacao.entrada_3
		                            , marcacao.saida_3
		                            , marcacao.entrada_4
		                            , marcacao.saida_4
		                            , marcacao.entrada_5
		                            , marcacao.saida_5
		                            , marcacao.entrada_6
		                            , marcacao.saida_6
		                            , marcacao.entrada_7
		                            , marcacao.saida_7
		                            , marcacao.entrada_8
		                            , marcacao.saida_8
									, empresa.id ID_Empresa
									FROM funcionario
                            INNER JOIN empresa ON empresa.id = funcionario.idempresa
                            INNER JOIN departamento ON departamento.id = funcionario.iddepartamento
                            INNER JOIN marcacao_view AS marcacao ON marcacao.idfuncionario = funcionario.id AND marcacao.data = @datainicial
                            WHERE funcionario.id > 0";

            switch (tipo)
            {
                case 0:
                    aux += @" AND empresa.id IN " + empresas;
                    break;
                case 1:
                    aux += @" AND empresa.id IN " + empresas + " AND departamento.id IN " + departamentos;
                    break;
                case 2:
                    aux += @" AND funcionario.id IN " + funcionarios;
                    break;
            }
            aux += @" ORDER BY empresa.nome, departamento.descricao, funcionario.nome";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            dt.Load(dr);
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return dt;
        }

        public DataTable GetRelatorioAbsenteismo(int tipo, string empresas, string departamentos, string funcionarios)
        {
            DataTable dt = new DataTable();
            string aux = @"SELECT funcionario.idempresa
                        , funcionario.iddepartamento
                        , funcionario.id AS idfuncionario
                        , funcionario.idfuncao
                        , convert(varchar,empresa.codigo)+' | '+empresa.nome as empresa
                        , convert(varchar,departamento.codigo)+' | '+departamento.descricao AS departamento
                        , convert(varchar,funcao.codigo) + ' | ' + funcao.descricao AS funcao
                        , coalesce(convert(varchar,cwu.codigo) + ' | ' + cwu.nome, '') AS supervisor
                        , coalesce(convert(varchar, pe.codigo) + ' | ' + pe.razaosocial, '') as PessoaSupervisor
                        , funcionario.dscodigo
                        , funcionario.nome AS funcionario
                        FROM funcionario
                        INNER JOIN empresa ON empresa.id = funcionario.idempresa
                        INNER JOIN departamento ON departamento.id = funcionario.iddepartamento
                        LEFT JOIN funcao ON funcao.id = funcionario.idFuncao
                        LEFT JOIN cw_usuario cwu ON cwu.id = funcionario.idcw_usuario
                        LEFT JOIN pessoa pe ON pe.id = funcionario.IdPessoaSupervisor
                        WHERE ISNULL(funcionario.excluido,0) = 0 AND funcionario.funcionarioativo = 1";

            switch (tipo)
            {
                case 0:
                    aux += @" AND empresa.id IN " + empresas;
                    break;
                case 1:
                    aux += @" AND empresa.id IN " + empresas + " AND departamento.id IN " + departamentos;
                    break;
                case 2:
                    aux += @" AND funcionario.id IN " + funcionarios;
                    break;
            }
            aux += @" ORDER BY empresa.nome, departamento.descricao, funcionario.nome";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, null);
            dt.Load(dr);
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return dt;
        }

        #endregion

        #region Listagens em DataTables

        public DataTable GetAll(bool pegaTodos)
        {
            SqlParameter[] parms = new SqlParameter[] { };

            string aux = @"SELECT funcionario.id,
                                           funcionario.codigo,
                                           funcionario.dscodigo,
                                           funcionario.matricula,
                                           funcionario.nome,
                                           funcionario.codigofolha,
                                           funcionario.idempresa,
                                           funcionario.iddepartamento,
                                           funcionario.idfuncao,
                                           funcionario.idhorario,
                                           funcionario.tipohorario,
                                           funcionario.carteira,
                                           funcionario.dataadmissao,
                                           funcionario.datademissao,
                                           funcionario.salario,
                                           funcionario.funcionarioativo,
                                           funcionario.DataInativacao,
                                           funcionario.naoentrarbanco,
                                           funcionario.naoentrarcompensacao,
                                           funcionario.excluido,
                                           funcionario.campoobservacao,
                                           funcionario.incdata,
                                           funcionario.inchora,
                                           funcionario.incusuario,
                                           funcionario.altdata,
                                           funcionario.althora,
                                           funcionario.altusuario,
                                           funcionario.pis,
                                           funcionario.senha,
                                           funcionario.toleranciaentrada,
                                           funcionario.toleranciasaida,
                                           funcionario.quantidadetickets,
                                           funcionario.tipotickets,
                                           funcionario.CPF,
                                           funcionario.Mob_Senha,
                                           funcionario.idcw_usuario,
                                           funcionario.utilizaregistrador,
                                           funcionario.UtilizaAppPontofopag,
                                           funcionario.UtilizaReconhecimentoFacialApp,
                                           funcionario.UtilizaWebAppPontofopag,
                                           funcionario.UtilizaReconhecimentoFacialWebApp,
                                           funcionario.idIntegracao,
                                           funcionario.IdPessoaSupervisor,
                                           funcionario.TipoMaoObra,
                                           funcionario.IdAlocacao,
                                           funcionario.IdTipoVinculo,
                                           funcionario.Email,
                                           funcionario.IdIntegracaoPainel,
                                           funcionario.RFID,
                         , horario.descricao AS jornada
                         , convert(varchar,empresa.codigo)+' | '+empresa.nome as empresa
                          FROM funcionario 
                          LEFT JOIN empresa  ON empresa.id = funcionario.idempresa
                          LEFT JOIN horario  ON horario.id = funcionario.idhorario";
            if (!pegaTodos)
            {
                aux += " WHERE COALESCE(funcionario.excluido,0)=0 AND COALESCE(funcionario.funcionarioativo,0)=1";
            }

            aux += " ORDER BY funcionario.nome";

            DataTable dt = new DataTable();

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            dt.Load(dr);
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return dt;
        }

        public DataTable GetPorEmpresa(int pIDEmpresa, bool pPegaInativos)
        {
            DataTable dt = new DataTable();
            SqlDataReader dr = GetEmpresaDR(pIDEmpresa, pPegaInativos);
            dt.Load(dr);
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();
            DataRow row = dt.NewRow();
            return dt;
        }

        public DataTable GetPorDepartamento(int pIDEmpresa, int pIDDepartamento, bool pPegaInativos)
        {
            DataTable dt = new DataTable();
            SqlDataReader dr = GetDepartamentoDR(pIDEmpresa, pIDDepartamento, pPegaInativos);
            dt.Load(dr);
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();
            return dt;
        }

        public DataTable GetFuncionariosAtivos()
        {
            DataTable dt = new DataTable();
            SqlDataReader dr = GetAtivosDR();
            dt.Load(dr);
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();
            return dt;
        }

        public DataTable GetParaProvisorio()
        {
            SqlParameter[] parms = new SqlParameter[0];
            DataTable dt = new DataTable();
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, SELECTPRO, parms);
            dt.Load(dr);
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return dt;
        }

        public DataTable GetExcluidos()
        {
            SqlParameter[] parms = new SqlParameter[0];

            DataTable dt = new DataTable();
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, SELECTDEL, parms);
            dt.Load(dr);
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return dt;
        }

        public DataTable GetParaDSR(int? pTipo, int pIdentificacao)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@identificacao", SqlDbType.Int)
            };
            parms[0].Value = pIdentificacao;

            DataTable dt = new DataTable();

            string comando = "SELECT funcionario.id, funcionario.nome, funcionario.idhorario FROM funcionario";

            if (pTipo != null)
            {
                switch (pTipo.Value)
                {
                    case 0://Empresa
                        comando += " WHERE funcionario.idempresa = @identificacao";
                        break;
                    case 1://Departamento
                        comando += " WHERE funcionario.iddepartamento = @identificacao";
                        break;
                    case 2://Funcionário
                        comando += " WHERE funcionario.id = @identificacao";
                        break;
                    case 3://Função
                        comando += " WHERE funcionario.idfuncao = @identificacao";
                        break;
                    case 4://Horário
                        comando += " WHERE funcionario.idhorario = @identificacao";
                        break;
                    default:
                        break;
                }
            }
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, comando, parms);
            dt.Load(dr);
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return dt;
        }

        /// <summary>
        /// Retorna os ids dos funcionário relacionados a o tipo passado como parâmetro
        /// </summary>
        /// <param name="pTipo">0:Empresa, 1:Departamento, 2:Funcionário,3:Função,4:Horário</param>
        /// <param name="pIdentificacao">Id do registro passado no tipo</param>
        /// <param name="pegaExcluidos">Indica se deseja retornar os funcionário excluídos</param>
        /// <param name="pegaInativos">Indica se deseja retornar os funcionários inativos</param>
        /// <returns>Retorna a lista dos ids dos funcionários</returns>
        public List<int> GetIDsByTipo(int? pTipo, List<int> pIdentificacoes, bool pegaExcluidos, bool pegaInativos)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@identificacao", SqlDbType.VarChar)
            };
            parms[0].Value = String.Join(",", pIdentificacoes);

            DataTable dt = new DataTable();

            string comando = "SELECT funcionario.id FROM funcionario WHERE 1 = 1 ";

            if (pTipo != null)
            {
                switch (pTipo.Value)
                {
                    case 0://Empresa
                        comando += " AND funcionario.idempresa in (select * from F_ClausulaIn(@identificacao))";
                        break;
                    case 1://Departamento
                        comando += " AND funcionario.iddepartamento in (select * from F_ClausulaIn(@identificacao))";
                        break;
                    case 2://Funcionário
                        comando += " AND funcionario.id in (select * from F_ClausulaIn(@identificacao))";
                        break;
                    case 3://Função
                        comando += " AND funcionario.idfuncao in (select * from F_ClausulaIn(@identificacao))";
                        break;
                    case 4://Horário
                        comando += " AND funcionario.idhorario in (select * from F_ClausulaIn(@identificacao))";
                        break;
                    default:
                        break;
                }
            }

            if (!pegaExcluidos) comando += " AND excluido = 0 ";

            if (!pegaInativos) comando += " AND funcionarioativo = 1 ";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, comando, parms);
            dt.Load(dr);

            if (pTipo.GetValueOrDefault() == 4)
            {
                SqlParameter[] parms2 = new SqlParameter[] { new SqlParameter("@identificacao", SqlDbType.VarChar) };
                parms2[0].Value = String.Join(",", pIdentificacoes);

                string sqlHorario = @"  SELECT distinct m.idfuncionario 
                                          FROM marcacao m
                                         inner join funcionario f on f.id = m.idfuncionario
                                         WHERE 1 = 1 AND m.idhorario in (select * from F_ClausulaIn(@identificacao))
                                           and idFechamentoPonto is null and idfechamentobh is null ";
                if (!pegaExcluidos) sqlHorario += " AND f.excluido = 0 ";
                if (!pegaInativos) sqlHorario += " AND f.funcionarioativo = 1 ";

                dr = db.ExecuteReader(CommandType.Text, sqlHorario, parms2);
                if (dr.HasRows)
                {
                    DataTable dt2 = new DataTable();
                    dt2.Load(dr);
                    dt2.AsEnumerable().Where(r => !dt.AsEnumerable().Select(s => s.ItemArray[0]).Contains(r.ItemArray[0])).ToList().ForEach(r => dt.ImportRow(r));
                }
            }

            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return dt.AsEnumerable().Select(x => Convert.ToInt32(x[0])).ToList();
        }

        #endregion

        #region DataReader

        public SqlDataReader GetTabelaMarcacaoDR(int tipo, int identificacao, string consultaNomeFuncionario)
        {
            SqlParameter[] parms = new SqlParameter[2]
                {
                    new SqlParameter("@identificacao", SqlDbType.Int),
                    new SqlParameter("@nome", SqlDbType.VarChar)
                };

            parms[0].Value = identificacao;
            parms[1].Value = consultaNomeFuncionario;
            string comando = @"   
            SELECT   
                func.altdata
                ,func.althora
                ,func.altusuario
                ,func.campoobservacao
                ,func.carteira
                ,func.codigo
                ,func.codigofolha
                ,func.CPF
                ,func.dataadmissao
                ,func.datademissao
                ,func.dscodigo
                ,func.Email
                ,func.excluido
                ,func.funcionarioativo
                ,func.DataInativacao
                ,func.id
                ,func.IdAlocacao
                ,func.idcw_usuario
                ,func.iddepartamento
                ,func.idempresa
                ,func.idfuncao
                ,func.idhorario
                ,func.idIntegracao
                ,func.IdIntegracaoPainel
                ,func.IdPessoaSupervisor
                ,func.IdTipoVinculo
                ,func.incdata
                ,func.inchora
                ,func.incusuario
                ,func.matricula
                ,func.Mob_Senha
                ,func.naoentrarbanco
                ,func.naoentrarcompensacao
                ,func.nome
                ,func.pis
                ,func.quantidadetickets
                ,func.salario
                ,func.senha
                ,func.tipohorario
                ,func.TipoMaoObra
                ,func.tipotickets
                ,func.toleranciaentrada
                ,func.toleranciasaida
                ,func.utilizaregistrador
                ,func.UtilizaAppPontofopag
                ,func.UtilizaReconhecimentoFacialApp
                ,func.UtilizaWebAppPontofopag
                ,func.UtilizaReconhecimentoFacialWebApp
                ,func.RFID
                ,func.IdHorarioDinamico
                ,func.CicloSequenciaIndice
                ,convert(varchar,HDN.codigo) +' | '+HDN.descricao HorarioDinamico
                ,'' foto
                ,'' contrato
                , CAST(func.dscodigo AS BIGINT) AS dscodigo 
                , horario.descricao AS jornada
                , convert(varchar,emp.codigo)+' | '+emp.nome as empresa
                , convert(varchar,departamento.codigo)+' | '+departamento.descricao AS departamento
                , convert(varchar,funcao.codigo) + ' | ' + funcao.descricao AS funcao
                , coalesce(convert(varchar,cwu.codigo) + ' | ' + cwu.nome, '') AS supervisor
                , coalesce(convert(varchar, pe.codigo) + ' | ' + pe.razaosocial, '') as PessoaSupervisor
                , convert(varchar,alocacao.codigo)+' | '+alocacao.descricao as Alocacao
                , convert(varchar,TipoVinculo.codigo)+' | '+TipoVinculo.descricao as TipoVinculo
            FROM funcionario func
            LEFT JOIN empresa emp ON emp.id = func.idempresa
            LEFT JOIN departamento ON departamento.id = func.iddepartamento
            LEFT JOIN funcao ON funcao.id = func.idFuncao
            LEFT JOIN cw_usuario cwu ON cwu.id = func.idcw_usuario
            LEFT JOIN pessoa pe ON pe.id = func.IdPessoaSupervisor
            LEFT JOIN horario ON horario.id = func.idhorario
            LEFT JOIN HorarioDinamico HDN ON HDN.id = func.idhorariodinamico
            LEFT JOIN alocacao on alocacao.id = func.idalocacao
            LEFT JOIN TipoVInculo on TipoVInculo.id = func.IdTipoVInculo";

            switch (tipo)
            {
                case 1:
                    comando += " WHERE func.idempresa = @identificacao";
                    break;
                case 2:
                    comando += " WHERE func.iddepartamento = @identificacao";
                    break;
                case 3:
                    comando += " WHERE func.id > 0";
                    break;
                case 5:
                    comando += " WHERE func.id in (select cf.idfuncionario from contratofuncionario cf where cf.idcontrato = @identificacao and cf.excluido = 0)";
                    break;
            }

            comando += " AND func.funcionarioativo = 1 AND ISNULL(func.excluido, 0) = 0";

            if (!String.IsNullOrEmpty(consultaNomeFuncionario))
            {
                comando += " and func.nome like '%' +@nome + '%'";
            }

            comando += PermissaoUsuarioFuncionario(UsuarioLogado, comando, "func.idempresa", "func.id", null);
            comando += " ORDER BY LOWER(func.nome)";

            return db.ExecuteReader(CommandType.Text, comando, parms);
        }

        /// <summary>
        /// Método que retorna uma lista de funcionários de uma determinada empresa
        /// </summary>
        /// <param name="pIDEmpresa">Id da empresa que deseja a lista de funcionários</param>
        /// <param name="pPegaInativos">False = pega apenas funcionarios ativos, true = Pega ativos e inativos.</param>
        /// <returns>Lista (SqlDataReader) dos funcionários</returns>
        public SqlDataReader GetEmpresaDR(int pIDEmpresa, bool pPegaInativos)
        {
            SqlParameter[] parms = new SqlParameter[1]
            {
                  new SqlParameter("@idempresa", SqlDbType.Int)
            };
            parms[0].Value = pIDEmpresa;

            string comando;
            if (pPegaInativos)
                comando = @"SELECT id, dscodigo, pis, tipohorario, idhorario, nome 
                            FROM funcionario WHERE idempresa = @idempresa AND ISNULL(excluido, 0) = 0 ";
            else
                comando = @"SELECT id, dscodigo, pis, tipohorario, idhorario, nome 
                            FROM funcionario WHERE idempresa = @idempresa AND funcionarioativo = 1 AND ISNULL(excluido, 0) = 0 AND datademissao IS NULL ";

            comando += " ORDER BY nome";

            return db.ExecuteReader(CommandType.Text, comando, parms);
        }

        /// <summary>
        /// Método que retorna uma lista de funcionários de um determinado departamento
        /// </summary>
        /// <param name="pIDEmpresa">ID da empresa</param>
        /// <param name="pIDDepartamento">ID do departamento</param>
        /// <param name="pPegaInativos">false = Pega somente ativos, true = Pega ativos e inativos</param>
        /// <returns>Lista (SqlDataReader) dos funcionários</returns>
        public SqlDataReader GetDepartamentoDR(int pIDEmpresa, int pIDDepartamento, bool pPegaInativos)
        {
            string comando;
            SqlParameter[] parms = new SqlParameter[2]
            {
                    new SqlParameter("@idempresa", SqlDbType.Int)
                  , new SqlParameter("@iddepartamento", SqlDbType.Int)
            };
            parms[0].Value = pIDEmpresa;
            parms[1].Value = pIDDepartamento;

            if (pPegaInativos)
                comando = @"SELECT id, dscodigo, tipohorario, idhorario, nome, pis FROM funcionario WHERE idempresa = @idempresa AND iddepartamento = @iddepartamento AND ISNULL(excluido, 0) = 0 ";
            else
                comando = @"SELECT id, dscodigo, tipohorario, idhorario, nome, pis FROM funcionario WHERE idempresa = @idempresa AND iddepartamento = @iddepartamento AND ISNULL(excluido, 0) = 0 AND funcionarioativo = 1  AND datademissao IS NULL ";
            comando += " ORDER BY nome";

            return db.ExecuteReader(CommandType.Text, comando, parms);
        }

        public SqlDataReader GetFuncaoDR(int pIDFuncao, bool pPegaInativos)
        {
            SqlParameter[] parms = new SqlParameter[1]
            {
                  new SqlParameter("@idfuncao", SqlDbType.Int)
            };
            parms[0].Value = pIDFuncao;

            string comando;
            if (pPegaInativos)
                comando = @"SELECT funcionario.id, funcionario.dscodigo, funcionario.pis, funcionario.tipohorario, funcionario.idhorario, funcionario.nome 
                            FROM funcionario 
                            INNER JOIN empresa emp ON emp.id = funcionario.idempresa
                            WHERE funcionario.idfuncao = @idfuncao AND ISNULL(funcionario.excluido, 0) = 0 ";
            else
                comando = @"SELECT funcionario.id, funcionario.dscodigo, funcionario.pis, funcionario.tipohorario, funcionario.idhorario, funcionario.nome 
                            FROM funcionario 
                            INNER JOIN empresa emp ON emp.id = funcionario.idempresa
                            WHERE funcionario.idfuncao = @idfuncao AND funcionario.funcionarioativo = 1 AND ISNULL(funcionario.excluido, 0) = 0 AND funcionario.datademissao IS NULL ";
            comando += " ORDER BY nome";

            return db.ExecuteReader(CommandType.Text, comando, parms);
        }

        public SqlDataReader GetAtivosDR()
        {
            SqlParameter[] parms = new SqlParameter[0];
            string comando = @"   SELECT   func.id
                                    , func.nome
                                    , CAST(func.dscodigo AS BIGINT) AS dscodigo 
                                    , func.matricula
                                    , horario.descricao AS jornada
                                    , convert(varchar,emp.codigo)+' | '+emp.nome as empresa
                                    , convert(varchar,departamento.codigo)+' | '+departamento.descricao AS departamento
                                    , convert(varchar,funcao.codigo) + ' | ' + funcao.descricao AS funcao
                                    , coalesce(convert(varchar,cwu.codigo) + ' | ' + cwu.nome, '') AS supervisor
                                    , coalesce(convert(varchar, pe.codigo) + ' | ' + pe.razaosocial, '') as PessoaSupervisor
                                    , func.carteira
                                    , func.dataadmissao
                                    , func.funcionarioativo
                             FROM funcionario func
                             LEFT JOIN empresa emp ON emp.id = func.idempresa
                             LEFT JOIN departamento ON departamento.id = func.iddepartamento
                             LEFT JOIN horario ON horario.id = func.idhorario
                             LEFT JOIN funcao ON funcao.id = func.idFuncao
                             LEFT JOIN cw_usuario cwu ON cwu.id = func.idcw_usuario
                             LEFT JOIN pessoa pe ON pe.id = func.IdPessoaSupervisor
                             WHERE func.funcionarioativo = 1 AND ISNULL(func.excluido, 0) = 0 
                             ORDER BY func.nome";

            return db.ExecuteReader(CommandType.Text, comando, parms);

        }

        /// <summary>
        /// Método que retorna uma lista de funcionários de um determinado departamento
        /// </summary>
        /// <param name="pIDEmpresa">ID da empresa</param>
        /// <param name="pIDDepartamento">ID do departamento</param>
        /// <returns>Lista (SqlDataReader) dos funcionários</returns>
        //PAM - 11/12/2009
        public SqlDataReader GetDepDR(int pIDEmpresa, int pIDDepartamento, bool pPegaInativos)
        {
            SqlParameter[] parms = new SqlParameter[1]
            {
                  new SqlParameter("@iddepartamento", SqlDbType.Int)
            };
            parms[0].Value = pIDDepartamento;

            string comando;
            if (pPegaInativos)
                comando = "SELECT id, dscodigo, tipohorario, idhorario, nome FROM funcionario WHERE iddepartamento = @iddepartamento AND COALESCE(funcionario.excluido, 0) = 0  ORDER BY nome";
            else
                comando = "SELECT id, dscodigo, tipohorario, idhorario, nome FROM funcionario WHERE iddepartamento = @iddepartamento AND COALESCE(funcionario.funcionarioativo,0) = 1 AND COALESCE(funcionario.excluido, 0) = 0  ORDER BY nome";

            return db.ExecuteReader(CommandType.Text, comando, parms);
        }

        /// <summary>
        /// Método que retorna o funcionário escolhido para mudança de horário
        /// </summary>
        /// <param name="pID">Id do funcionário</param>
        /// <returns>Lista (SqlDataReader) dos funcionários</returns>
        public SqlDataReader GetIDDR(List<int> pIDs)
        {
            SqlParameter[] parms = new SqlParameter[1]
            {
                  new SqlParameter("@ids", SqlDbType.VarChar)
            };
            parms[0].Value = String.Join(",", pIDs);

            string aux = @"SELECT id, dscodigo, tipohorario, idhorario, nome, pis FROM funcionario WHERE id in (select * from F_ClausulaIn(@ids))";

            return db.ExecuteReader(CommandType.Text, aux, parms);
        }

        #endregion

        #region Auxiliares

        /// <summary>
        /// Pega o numero de funcionarios ativos de todo o banco de dados que esta sendo pesquisado
        /// </summary>
        /// <returns>Numero de funcionarios do banco</returns>
        public int GetNumFuncionarios()
        {
            SqlParameter[] parms = new SqlParameter[0] { };

            //Select considerando somente os funcionários ativos
            //22/12/2009 - crnc
            string aux = @" SELECT COUNT(1) 
                            FROM funcionario
                            WHERE ISNULL(funcionarioativo, 0) = 1
                            AND ISNULL(excluido, 0) = 0 ";

            return (int)db.ExecuteScalar(CommandType.Text, aux, parms);
        }

        public bool MudaCodigoFuncionario(int pFuncionarioID, string pCodigoNovo, DateTime pData)
        {
            //Verifica se o código já está atribuido para algun funcionário
            if (this.VerificaCodigoDuplicado(pCodigoNovo) == true)
            {
                throw new Exception("O código " + pCodigoNovo + " já está sendo utilizado por outro funcionário.");
            }

            //Rotina que troca o código do funcionário
            using (SqlConnection conn = db.GetConnection)
            {
                Modelo.Funcionario objFunc = this.LoadObject(pFuncionarioID);
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        //Altera o código do funcionario
                        SqlParameter[] parms = new SqlParameter[]
                        {
                              new SqlParameter("@id", SqlDbType.Int)
                            , new SqlParameter("@dscodigo", SqlDbType.VarChar)
                            , new SqlParameter("@data", SqlDbType.DateTime)
                        };
                        parms[0].Value = pFuncionarioID;
                        parms[1].Value = pCodigoNovo;
                        parms[2].Value = pData;

                        string aux = "UPDATE funcionario SET dscodigo = @dscodigo WHERE id = @id";

                        string aux1 = "UPDATE marcacao SET dscodigo = @dscodigo WHERE marcacao.idfuncionario = @id AND marcacao.data >= @data";

                        TransactDbOps.ExecNonQueryCmd(trans, CommandType.Text, aux + "; " + aux1 + ";", true, parms);

                        //Registra a mudança do código do funcionário
                        Modelo.MudCodigoFunc objMudanca = new Modelo.MudCodigoFunc();
                        objMudanca.Codigo = dalMudanca.MaxCodigo();
                        objMudanca.IdFuncionario = objFunc.Id;
                        objMudanca.DSCodigoAntigo = objFunc.Dscodigo;
                        objMudanca.DSCodigoNovo = pCodigoNovo;
                        if (pData != System.DateTime.Today)
                            objMudanca.Datainicial = pData;
                        else
                            objMudanca.Datainicial = System.DateTime.Today;
                        objMudanca.Idempresa = objFunc.Idempresa;
                        objMudanca.Iddepartamento = objFunc.Iddepartamento;
                        objMudanca.Idhorarionormal = objFunc.Idhorario;
                        objMudanca.Tipohorario = objFunc.Tipohorario;

                        dalMudanca.Incluir(trans, objMudanca);

                        trans.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        throw (ex);
                    }
                }
            }
        }

        protected override void IncluirAux(SqlTransaction trans, Modelo.ModeloBase obj)
        {
            SetDadosInc(obj);
            SqlParameter[] parms = GetParameters();
            SetParameters(parms, obj);

            if (TransactDbOps.CountCampo(trans, TABELA, "dscodigo", Convert.ToDouble(((Modelo.Funcionario)obj).Dscodigo), 0) > 0)
            {
                throw new Exception("O código informado já está sendo utilizado em outro registro. Verifique.");
            }

            SqlCommand cmd = TransactDbOps.ExecNonQueryCmd(trans, CommandType.Text, INSERT, true, parms);
            obj.Id = Convert.ToInt32(cmd.Parameters["@id"].Value);

            AuxManutencao(trans, obj);

            cmd.Parameters.Clear();
        }

        protected override void AlterarAux(SqlTransaction trans, Modelo.ModeloBase obj)
        {
            SetDadosAlt(obj);
            SqlParameter[] parms = GetParameters();
            SetParameters(parms, obj);

            if (TransactDbOps.CountCampo(trans, TABELA, "dscodigo", Convert.ToDouble(((Modelo.Funcionario)obj).Dscodigo), ((Modelo.ModeloBase)obj).Id) > 0)
            {
                throw new Exception("O código informado já está sendo utilizado em outro registro. Verifique.");
            }

            SqlCommand cmd = TransactDbOps.ExecNonQueryCmd(trans, CommandType.Text, UPDATE, true, parms);

            AuxManutencao(trans, obj);

            cmd.Parameters.Clear();
        }

        private void AuxManutencao(SqlTransaction trans, Modelo.ModeloBase obj)
        {
            foreach (Modelo.FuncionarioHistorico fh in ((Modelo.Funcionario)obj).Historico)
            {
                fh.Idfuncionario = ((Modelo.Funcionario)obj).Id;
                switch (fh.Acao)
                {
                    case Modelo.Acao.Incluir:
                        dalFuncionarioHistorico.Incluir(trans, fh);
                        break;
                    case Modelo.Acao.Alterar:
                        dalFuncionarioHistorico.Alterar(trans, fh);
                        break;
                    case Modelo.Acao.Excluir:
                        dalFuncionarioHistorico.Excluir(trans, fh);
                        break;
                }
            }

            Modelo.Funcionario RFIDfuncionario = ((Modelo.Funcionario)obj);
            if (RFIDfuncionario.RFID != RFIDfuncionario.RFID_Ant)
            {
                DAL.SQL.FuncionarioRFID dalFuncionarioRFID = new DAL.SQL.FuncionarioRFID(db);
                dalFuncionarioRFID.UsuarioLogado = UsuarioLogado;
                Modelo.FuncionarioRFID RFID = new Modelo.FuncionarioRFID()
                {
                    RFID = RFIDfuncionario.RFID.GetValueOrDefault(),
                    IdFuncionario = RFIDfuncionario.Id,
                    Codigo = dalFuncionarioRFID.MaxCodigo()
                };

                dalFuncionarioRFID.Incluir(trans, RFID);
            }

        }

        public void Incluir(List<Modelo.Funcionario> funcionarios, bool salvarHistorico)
        {
            SqlParameter[] parms = GetParameters();
            using (SqlConnection conn = db.GetConnection)
            {
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        SqlCommand cmd;
                        foreach (Modelo.Funcionario func in funcionarios)
                        {
                            SetDadosInc(func);
                            SetParameters(parms, func);
                            cmd = TransactDbOps.ExecNonQueryCmd(trans, CommandType.Text, INSERT, true, parms);
                            func.Id = Convert.ToInt32(cmd.Parameters["@id"].Value);
                            if (salvarHistorico)
                                AuxManutencao(trans, func);

                            cmd.Parameters.Clear();
                        }
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        conn.Close();
                        throw (ex);
                    }
                }
            }
        }

        #endregion

        #region Listagem em List

        public List<int> GetIds()
        {
            List<int> lista = new List<int>();

            string comando = "SELECT func.id FROM funcionario func";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, comando, new SqlParameter[] { });

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    lista.Add(Convert.ToInt32(dr["id"]));
                }
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return lista;
        }

        /// <summary>
        ///    Retorna lista de ids
        /// </summary>
        /// <param name="condicao"> Concatena condicional no select</param>
        /// <returns></returns>
        public List<int> GetIdsFuncsAtivos(string condicao)
        {
            List<int> lista = new List<int>();

            string comando = " SELECT id FROM funcionario where funcionarioativo = 1 AND excluido = 0 " + condicao;

            //if (Modelo.cwkGlobal.objUsuarioLogado.Tipo != 0)
            //{
            //    comando += " WHERE (SELECT COUNT(id) FROM empresausuario WHERE empresausuario.idusuario = "
            //        + Modelo.cwkGlobal.objUsuarioLogado.Id.ToString() + " AND empresausuario.idempresa = funcionario.idempresa) > 0";
            //}

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, comando, new SqlParameter[] { });

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    lista.Add(Convert.ToInt32(dr["id"]));
                }
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return lista;
        }

        public List<Modelo.Funcionario> GetAllList(bool pegaTodos)
        {
            List<Modelo.Funcionario> lista = new List<Modelo.Funcionario>();

            SqlParameter[] parms = new SqlParameter[] { };

            string aux = @"SELECT   func.id,
                                   func.codigo,
                                   func.dscodigo,
                                   func.matricula,
                                   func.nome,
                                   func.codigofolha,
                                   func.idempresa,
                                   func.iddepartamento,
                                   func.idfuncao,
                                   func.idhorario,
                                   func.tipohorario,
                                   func.carteira,
                                   func.dataadmissao,
                                   func.datademissao,
                                   func.salario,
                                   func.funcionarioativo,
                                   func.DataInativacao,
                                   func.naoentrarbanco,
                                   func.naoentrarcompensacao,
                                   func.excluido,
                                   func.campoobservacao,
                                   func.incdata,
                                   func.inchora,
                                   func.incusuario,
                                   func.altdata,
                                   func.althora,
                                   func.altusuario,
                                   func.pis,
                                   func.senha,
                                   func.toleranciaentrada,
                                   func.toleranciasaida,
                                   func.quantidadetickets,
                                   func.tipotickets,
                                   func.CPF,
                                   func.Mob_Senha,
                                   func.idcw_usuario,
                                   func.utilizaregistrador,
                                   func.UtilizaAppPontofopag,
                                   func.UtilizaReconhecimentoFacialApp,
                                   func.UtilizaWebAppPontofopag,
                                   func.UtilizaReconhecimentoFacialWebApp,
                                   func.idIntegracao,
                                   func.IdPessoaSupervisor,
                                   func.TipoMaoObra,
                                   func.IdAlocacao,
                                   func.IdTipoVinculo,
                                   func.Email,
                                   func.IdIntegracaoPainel,
                                   func.RFID,
                                    '' foto,
                                    '' contrato
                                    , horario.descricao AS jornada
                                    , convert(varchar,empresa.codigo)+' | '+ empresa.nome as empresa  
                                    , convert(varchar,departamento.codigo)+' | '+ departamento.descricao AS departamento
                                    , convert(varchar,funcao.codigo) + ' | ' + funcao.descricao AS funcao
                                    , coalesce(convert(varchar,cwu.codigo) + ' | ' + cwu.nome, '') AS supervisor
                                    , coalesce(convert(varchar, pe.codigo) + ' | ' + pe.razaosocial, '') as PessoaSupervisor
                                    , convert(varchar,Alocacao.codigo) + ' | ' + Alocacao.descricao AS Alocacao
                                    , convert(varchar,TipoVinculo.codigo) + ' | ' + TipoVinculo.descricao AS TipoVinculo
                                    , func.IdHorarioDinamico
                                    , func.CicloSequenciaIndice
                                    , convert(varchar,HDN.codigo) +' | '+HDN.descricao HorarioDinamico
                             FROM funcionario func 
                             LEFT JOIN empresa ON empresa.id = func.idempresa
                             LEFT JOIN departamento ON departamento.id = func.iddepartamento
                             LEFT JOIN horario ON horario.id = func.idhorario
                             LEFT JOIN funcao ON funcao.id = func.idFuncao
                             LEFT JOIN cw_usuario cwu ON cwu.id = func.idcw_usuario
                             LEFT JOIN Alocacao ON alocacao.id = func.idAlocacao
                             LEFT JOIN pessoa pe ON pe.id = func.IdPessoaSupervisor
                             LEFT JOIN TipoVinculo ON TipoVinculo.id = func.IdTipoVinculo
                             LEFT JOIN HorarioDinamico HDN ON HDN.id = func.idhorariodinamico ";


            if (!pegaTodos)
            {
                aux += " WHERE ISNULL(func.excluido,0)=0 AND ISNULL(func.funcionarioativo,0)=1";
            }
            aux += PermissaoUsuarioFuncionario(UsuarioLogado, aux, "func.idempresa", "func.id", null);
            aux += " ORDER BY func.nome";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Modelo.Funcionario objFuncionario = new Modelo.Funcionario();
                    AuxSetInstance(dr, objFuncionario);

                    lista.Add(objFuncionario);
                }
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return lista;
        }

        public List<Modelo.Proxy.pxyFuncionarioGrid> GetAllGrid()
        {
            return GetAllGrid(1);
        }

        public List<Modelo.Proxy.pxyFuncionarioGrid> GetAllGrid(int flag)
        {
            List<Modelo.Proxy.pxyFuncionarioGrid> lista = new List<Modelo.Proxy.pxyFuncionarioGrid>();

            SqlParameter[] parms = new SqlParameter[] { };

            string aux = @" SELECT func.id Id,
		                            func.dscodigo Codigo,
		                            func.nome Nome,
		                            func.matricula Matricula,
		                            func.carteira Carteira,	
                                    uContr.contrato contrato,			
		                            func.CPF CPF,
		                            func.codigofolha CodigoFolha,
		                            func.pis Pis,
		                            CONVERT(VARCHAR(12),func.dataadmissao,103) DataAdmissao,
		                            CONVERT(VARCHAR(12),func.datademissao,103) DataDemissao,
		                            CASE WHEN func.funcionarioativo = 1 THEN 'Sim' ELSE 'Não' END Ativo,
                                    CONVERT(VARCHAR(12),func.DataInativacao,103) DataInativacao,
		                            CASE WHEN horario.tipohorario = 2 THEN 'Flexível' ELSE 'Normal' END TipoHorario,
		                            CASE WHEN func.naoentrarbanco = 0 THEN 'Sim' ELSE 'Não' END EntraBancoHoras,
		                            CASE WHEN func.naoentrarcompensacao = 0 THEN 'Sim' ELSE 'Não' END EntraCompensacao,
		                            CASE WHEN func.utilizaregistrador = 0 THEN 'Não' ELSE 'Sim' END Utilizaregistrador,
                                    CASE WHEN func.UtilizaAppPontofopag = 0 THEN 'Não' ELSE 'Sim' END UtilizaAppPontofopag,
                                    CASE WHEN func.UtilizaReconhecimentoFacialApp = 0 THEN 'Não' ELSE 'Sim' END UtilizaReconhecimentoFacialApp,
                                    CASE WHEN func.UtilizaWebAppPontofopag = 0 THEN 'Não' ELSE 'Sim' END UtilizaWebAppPontofopag,
                                    CASE WHEN func.UtilizaReconhecimentoFacialWebApp = 0 THEN 'Não' ELSE 'Sim' END UtilizaReconhecimentoFacialWebApp,
		                            CASE WHEN func.TipoMaoObra = 0 THEN 'Direta'
			                             WHEN func.TipoMaoObra = 1 THEN 'Indireta'
			                             WHEN func.TipoMaoObra = 2 THEN 'Mensalista' END TipoMaoObra,
		                            CONVERT(VARCHAR, horario.codigo) + ' | ' +horario.descricao AS Horario,
                                    CONVERT(VARCHAR, empresa.codigo) + ' | ' + empresa.nome AS Empresa ,
                                    CONVERT(VARCHAR, departamento.codigo) + ' | ' + departamento.descricao AS Departamento ,
                                    CONVERT(VARCHAR, funcao.codigo) + ' | ' + funcao.descricao AS Funcao ,
                                    COALESCE(CONVERT(VARCHAR, cwu.codigo) + ' | ' + cwu.nome, '') AS Supervisor ,
                                    COALESCE(CONVERT(VARCHAR, pe.codigo) + ' | ' + pe.RazaoSocial, '') AS PessoaSupervisor ,
                                    CONVERT(VARCHAR, Alocacao.codigo) + ' | ' + Alocacao.descricao AS Alocacao ,
                                    CONVERT(VARCHAR, TipoVinculo.codigo) + ' | ' + TipoVinculo.descricao AS TipoVinculo,
                                    func.RFID
                             FROM   funcionario func
                                    LEFT JOIN empresa ON empresa.id = func.idempresa
                                    LEFT JOIN departamento ON departamento.id = func.iddepartamento
                                    LEFT JOIN horario ON horario.id = func.idhorario
                                    LEFT JOIN funcao ON funcao.id = func.idfuncao
                                    LEFT JOIN cw_usuario cwu ON cwu.id = func.idcw_usuario
                                    LEFT JOIN Alocacao ON Alocacao.id = func.IdAlocacao
                                    LEFT JOIN Pessoa pe ON pe.id = func.IdPessoaSupervisor
                                    LEFT JOIN TipoVinculo ON TipoVinculo.id = func.IdTipoVinculo
									OUTER APPLY(SELECT TOP 1 cfun.inchora,CASE WHEN cont.codigo is null THEN '-' ELSE CONCAT(cont.codigo,' | ',cont.codigocontrato,' - ',cont.descricaocontrato) END contrato 
                                                FROM dbo.contratofuncionario cfun LEFT JOIN dbo.contrato cont ON cont.id = cfun.idcontrato 
												    WHERE func.id = cfun.idfuncionario and cfun.excluido =0 ) AS uContr                             
                                    WHERE  func.id in (select id from funcionario where funcionario.excluido = 0)
                                    ";

            if (flag == 0)
                aux += "and func.funcionarioativo = 0 ";
            else if (flag == 1)
                aux += "and func.funcionarioativo = 1 ";
            

            aux += PermissaoUsuarioFuncionario(UsuarioLogado, aux, "func.idempresa", "func.id", null);
            aux += " ORDER BY func.nome";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.Proxy.pxyFuncionarioGrid>();
                lista = AutoMapper.Mapper.Map<List<Modelo.Proxy.pxyFuncionarioGrid>>(dr);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (!dr.IsClosed)
                {
                    dr.Close();
                }
                dr.Dispose();
            }
            return lista;
        }

        public List<Modelo.Funcionario> GetAllListComDataUltimoFechamento(bool pegaTodos)
        {
            return GetAllListComDataUltimoFechamento(pegaTodos, new List<int>());
        }

        public List<Modelo.Funcionario> GetAllListComDataUltimoFechamento(bool pegaTodos, IList<int> idsFuncs)
        {
            List<Modelo.Funcionario> lista = new List<Modelo.Funcionario>();

            SqlParameter[] parms = new SqlParameter[1]
            {
                    new SqlParameter("@idsFuncs", SqlDbType.Structured)
            };
            parms[0].Value = CreateDataTableIdentificadores(idsFuncs.Select(s => (long)s));
            parms[0].TypeName = "Identificadores";


            string aux = @"SELECT  func.altdata,
                                   func.althora,
                                   func.altusuario,
                                   func.campoobservacao,
                                   func.carteira,
                                   func.codigo,
                                   func.codigofolha,
                                   func.CPF,
                                   func.dataadmissao,
                                   func.datademissao,
                                   func.dscodigo,
                                   func.Email,
                                   func.excluido,
                                   func.funcionarioativo,
                                   func.DataInativacao,
                                   func.id,
                                   func.IdAlocacao,
                                   func.idcw_usuario,
                                   func.iddepartamento,
                                   func.idempresa,
                                   func.idfuncao,
                                   func.idhorario,
                                   func.idIntegracao,
                                   func.IdIntegracaoPainel,
                                   func.IdPessoaSupervisor,
                                   func.IdTipoVinculo,
                                   func.incdata,
                                   func.inchora,
                                   func.incusuario,
                                   func.matricula,
                                   func.Mob_Senha,
                                   func.naoentrarbanco,
                                   func.naoentrarcompensacao,
                                   func.nome,
                                   func.pis,
                                   func.quantidadetickets,
                                   func.salario,
                                   func.senha,
                                   func.tipohorario,
                                   func.TipoMaoObra,
                                   func.tipotickets,
                                   func.toleranciaentrada,
                                   func.toleranciasaida,
                                   func.utilizaregistrador,
                                   func.UtilizaAppPontofopag,
                                   func.UtilizaReconhecimentoFacialApp,
                                   func.UtilizaWebAppPontofopag,
                                   func.UtilizaReconhecimentoFacialWebApp
                                    , horario.descricao AS jornada
                                    , convert(varchar,empresa.codigo)+' | '+ empresa.nome as empresa  
                                    , convert(varchar,departamento.codigo)+' | '+ departamento.descricao AS departamento
                                    , convert(varchar,funcao.codigo) + ' | ' + funcao.descricao AS funcao
                                    , coalesce(convert(varchar,cwu.codigo) + ' | ' + cwu.nome, '') AS supervisor
                                    , coalesce(convert(varchar, pe.codigo) + ' | ' + pe.razaosocial, '') as PessoaSupervisor
									, fp.DataUltimoFechamento
                             FROM @idsFuncs FI
                             INNER JOIN funcionario func ON FI.Identificador = func.id
                             LEFT JOIN empresa ON empresa.id = func.idempresa
                             LEFT JOIN departamento ON departamento.id = func.iddepartamento
                             LEFT JOIN horario ON horario.id = func.idhorario
                             LEFT JOIN funcao ON funcao.id = func.idFuncao
                             LEFT JOIN cw_usuario cwu ON cwu.id = func.idcw_usuario
                             LEFT JOIN pessoa pe ON pe.id = func.IdPessoaSupervisor
							 LEFT JOIN (select idfuncionario, max(datafechamento) DataUltimoFechamento 
										  from fechamentoponto fp
										 inner join fechamentopontofuncionario fpf on fp.id = fpf.idfechamentoponto 
										 group by idfuncionario) FP ON fp.idfuncionario = func.id
                             WHERE 1 = 1 ";


            if (!pegaTodos)
            {
                aux += " AND ISNULL(func.excluido,0)=0 AND ISNULL(func.funcionarioativo,0)=1";
            }

            aux += PermissaoUsuarioFuncionario(UsuarioLogado, aux, "func.idempresa", "func.id", null);
            aux += " ORDER BY func.nome";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.Funcionario>();
                lista = AutoMapper.Mapper.Map<List<Modelo.Funcionario>>(dr);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (!dr.IsClosed)
                {
                    dr.Close();
                }
                dr.Dispose();
            }
            return lista;
        }

        /// <summary>
        /// Retorna todos funcionários com a data do ultimo fechamento de banco e de ponto por funcionário
        /// </summary>
        /// <param name="pegaTodos">Indica se deseja que retorne os funcionários mesmo que inativos ou excluidos</param>
        /// <returns>Retorna lista de funcionários com a data do ultimo fechamento de banco e de ponto para o funcionário</returns>
        public List<Modelo.Funcionario> GetAllListComUltimosFechamentos(bool pegaTodos)
        {
            return GetAllListComUltimosFechamentos(pegaTodos, new List<int>());
        }

        /// <summary>
        /// Retorna os funcionários com a data do ultimo fechamento de banco e de ponto para o funcionário
        /// </summary>
        /// <param name="pegaTodos">Indica se deseja que retorne os funcionários mesmo que inativos ou excluidos</param>
        /// <param name="idsFuncs">ids dos funcionários a serem selecionados</param>
        /// <returns>Retorna lista de funcionários com a data do ultimo fechamento de banco e de ponto para o funcionário</returns>
        public List<Modelo.Funcionario> GetAllListComUltimosFechamentos(bool pegaTodos, IList<int> idsFuncs)
        {
            List<Modelo.Funcionario> lista = new List<Modelo.Funcionario>();

            SqlParameter[] parms = new SqlParameter[] { };

            string aux = @"SELECT   func.id,
                                   func.codigo,
                                   func.dscodigo,
                                   func.matricula,
                                   func.nome,
                                   func.codigofolha,
                                   func.idempresa,
                                   func.iddepartamento,
                                   func.idfuncao,
                                   func.idhorario,
                                   func.tipohorario,
                                   func.carteira,
                                   func.dataadmissao,
                                   func.datademissao,
                                   func.salario,
                                   func.funcionarioativo,
                                   func.DataInativacao,
                                   func.naoentrarbanco,
                                   func.naoentrarcompensacao,
                                   func.excluido,
                                   func.campoobservacao,
                                   func.incdata,
                                   func.inchora,
                                   func.incusuario,
                                   func.altdata,
                                   func.althora,
                                   func.altusuario,
                                   func.pis,
                                   func.senha,
                                   func.toleranciaentrada,
                                   func.toleranciasaida,
                                   func.quantidadetickets,
                                   func.tipotickets,
                                   func.CPF,
                                   func.Mob_Senha,
                                   func.idcw_usuario,
                                   func.utilizaregistrador,
                                   func.UtilizaAppPontofopag,
                                   func.UtilizaReconhecimentoFacialApp,
                                   func.UtilizaWebAppPontofopag,
                                   func.UtilizaReconhecimentoFacialWebApp,
                                   func.idIntegracao,
                                   func.IdPessoaSupervisor,
                                   func.TipoMaoObra,
                                   func.IdAlocacao,
                                   func.IdTipoVinculo,
                                   func.Email,
                                   func.IdIntegracaoPainel
                                    , horario.descricao AS jornada
                                    , convert(varchar,empresa.codigo)+' | '+ empresa.nome as empresa  
                                    , convert(varchar,departamento.codigo)+' | '+ departamento.descricao AS departamento
                                    , convert(varchar,funcao.codigo) + ' | ' + funcao.descricao AS funcao
                                    , coalesce(convert(varchar,cwu.codigo) + ' | ' + cwu.nome, '') AS supervisor
                                    , coalesce(convert(varchar, pe.codigo) + ' | ' + pe.razaosocial, '') as PessoaSupervisor
									, fp.DataUltimoFechamento
									, fb.ultimoFechamentoBH DataUltimoFechamentoBH
                             FROM funcionario func 
                             LEFT JOIN empresa ON empresa.id = func.idempresa
                             LEFT JOIN departamento ON departamento.id = func.iddepartamento
                             LEFT JOIN horario ON horario.id = func.idhorario
                             LEFT JOIN funcao ON funcao.id = func.idFuncao
                             LEFT JOIN cw_usuario cwu ON cwu.id = func.idcw_usuario
                             LEFT JOIN pessoa pe ON pe.id = func.IdPessoaSupervisor
							 OUTER APPLY (SELECT top(1) fbh.data ultimoFechamentoBH
										  FROM dbo.fechamentobh fbh
										 INNER JOIN dbo.fechamentobhd fbhd ON fbh.id = fbhd.idfechamentobh
										 WHERE fbhd.identificacao = func.id
										 ORDER BY fbh.data DESC) fb
							 OUTER APPLY (SELECT top(1) datafechamento DataUltimoFechamento
											FROM fechamentoponto fp
											INNER JOIN fechamentopontofuncionario fpf ON fp.id = fpf.idfechamentoponto 
											WHERE idfuncionario = func.id
											ORDER BY datafechamento DESC) fp
                             WHERE 1 = 1 ";

            if (!pegaTodos)
            {
                aux += " AND ISNULL(func.excluido,0)=0 AND ISNULL(func.funcionarioativo,0)=1";
            }

            if (idsFuncs.Count() > 0)
            {
                aux += "AND func.id in (" + String.Join(",", idsFuncs) + ")";
            }

            aux += PermissaoUsuarioFuncionario(UsuarioLogado, aux, "func.idempresa", "func.id", null);
            aux += " ORDER BY func.nome";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.Funcionario>();
                lista = AutoMapper.Mapper.Map<List<Modelo.Funcionario>>(dr);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (!dr.IsClosed)
                {
                    dr.Close();
                }
                dr.Dispose();
            }
            return lista;
        }

        public List<Modelo.Funcionario> GetAllListLike(bool pegaTodos, string nome)
        {
            List<Modelo.Funcionario> lista = new List<Modelo.Funcionario>();

            SqlParameter[] parms = new SqlParameter[]
            {
                 new SqlParameter("@nome", SqlDbType.VarChar)
            };
            parms[0].Value = nome;

            string aux = @"SELECT   func.id,
                                   func.codigo,
                                   func.dscodigo,
                                   func.matricula,
                                   func.nome,
                                   func.codigofolha,
                                   func.idempresa,
                                   func.iddepartamento,
                                   func.idfuncao,
                                   func.idhorario,
                                   func.tipohorario,
                                   func.carteira,
                                   func.dataadmissao,
                                   func.datademissao,
                                   func.salario,
                                   func.funcionarioativo,
                                   func.DataInativacao,
                                   func.naoentrarbanco,
                                   func.naoentrarcompensacao,
                                   func.excluido,
                                   func.campoobservacao,
                                   func.incdata,
                                   func.inchora,
                                   func.incusuario,
                                   func.altdata,
                                   func.althora,
                                   func.altusuario,
                                   func.pis,
                                   func.senha,
                                   func.toleranciaentrada,
                                   func.toleranciasaida,
                                   func.quantidadetickets,
                                   func.tipotickets,
                                   func.CPF,
                                   func.Mob_Senha,
                                   func.idcw_usuario,
                                   func.utilizaregistrador,
                                   func.UtilizaAppPontofopag,
                                   func.UtilizaReconhecimentoFacialApp,
                                   func.UtilizaWebAppPontofopag,
                                   func.UtilizaReconhecimentoFacialWebApp,
                                   func.idIntegracao,
                                   func.IdPessoaSupervisor,
                                   func.TipoMaoObra,
                                   func.IdAlocacao,
                                   func.IdTipoVinculo,
                                   func.Email,
                                   func.IdIntegracaoPainel,
                                   func.RFID, 
                                   '' foto,
                                   '' contrato
                                    , horario.descricao AS jornada
                                    , convert(varchar,empresa.codigo)+' | '+ empresa.nome as empresa  
                                    , convert(varchar,departamento.codigo)+' | '+ departamento.descricao AS departamento
                                    , convert(varchar,funcao.codigo) + ' | ' + funcao.descricao AS funcao
                                    , coalesce(convert(varchar,cwu.codigo) + ' | ' + cwu.nome, '') AS supervisor
                                    , coalesce(convert(varchar, pe.codigo) + ' | ' + pe.razaosocial, '') as PessoaSupervisor
                                    , convert(varchar,Alocacao.codigo) + ' | ' + Alocacao.descricao AS Alocacao
                                    , convert(varchar,TipoVinculo.codigo) + ' | ' + TipoVinculo.descricao AS TipoVinculo
                                    , func.IdHorarioDinamico
                                    , func.CicloSequenciaIndice
                                    , convert(varchar,HDN.codigo) +' | '+HDN.descricao HorarioDinamico
                             FROM funcionario func 
                             LEFT JOIN empresa ON empresa.id = func.idempresa
                             LEFT JOIN departamento ON departamento.id = func.iddepartamento
                             LEFT JOIN horario ON horario.id = func.idhorario
                             LEFT JOIN funcao ON funcao.id = func.idFuncao
                             LEFT JOIN cw_usuario cwu ON cwu.id = func.idcw_usuario
                             LEFT JOIN pessoa pe ON pe.id = func.IdPessoaSupervisor
                             LEFT JOIN Alocacao ON alocacao.id = func.idAlocacao
                             LEFT JOIN TipoVinculo ON TipoVinculo.id = func.idTipoVinculo
                             LEFT JOIN HorarioDinamico HDN ON HDN.id = func.idhorariodinamico
                            WHERE func.nome like '%'+@nome+'%'";


            if (!pegaTodos)
            {
                aux += " and ISNULL(func.excluido,0)=0 AND ISNULL(func.funcionarioativo,0)=1";
            }
            aux += PermissaoUsuarioFuncionario(UsuarioLogado, aux, "func.idempresa", "func.id", null);
            aux += " ORDER BY func.nome";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Modelo.Funcionario objFuncionario = new Modelo.Funcionario();
                    AuxSetInstance(dr, objFuncionario);

                    lista.Add(objFuncionario);
                }
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return lista;
        }

        public List<Modelo.Funcionario> GetAllListByIds(string funcionarios)
        {
            var _func = funcionarios.Replace("(", "").Replace(")", "");

            List<Modelo.Funcionario> lista = new List<Modelo.Funcionario>();
            
            SqlParameter[] parms = new SqlParameter[]
            {
                 new SqlParameter("@funcionarios", SqlDbType.VarChar)
            };
            parms[0].Value = _func;

            string aux =string.Format(@"SELECT   func.id,
                                   func.codigo,
                                   func.dscodigo,
                                   func.matricula,
                                   func.nome,
                                   func.codigofolha,
                                   func.idempresa,
                                   func.iddepartamento,
                                   func.idfuncao,
                                   func.idhorario,
                                   func.tipohorario,
                                   func.carteira,
                                   func.dataadmissao,
                                   func.datademissao,
                                   func.salario,
                                   func.funcionarioativo,
                                   func.DataInativacao,
                                   func.naoentrarbanco,
                                   func.naoentrarcompensacao,
                                   func.excluido,
                                   func.campoobservacao,
                                   func.incdata,
                                   func.inchora,
                                   func.incusuario,
                                   func.altdata,
                                   func.althora,
                                   func.altusuario,
                                   func.pis,
                                   func.senha,
                                   func.toleranciaentrada,
                                   func.toleranciasaida,
                                   func.quantidadetickets,
                                   func.tipotickets,
                                   func.CPF,
                                   func.Mob_Senha,
                                   func.idcw_usuario,
                                   func.utilizaregistrador,
                                   func.UtilizaAppPontofopag,
                                   func.UtilizaReconhecimentoFacialApp,
                                   func.UtilizaWebAppPontofopag,
                                   func.UtilizaReconhecimentoFacialWebApp,
                                   func.idIntegracao,
                                   func.IdPessoaSupervisor,
                                   func.TipoMaoObra,
                                   func.IdAlocacao,
                                   func.IdTipoVinculo,
                                   func.Email,
                                   func.IdIntegracaoPainel,
                                   func.RFID, 
                                    '' foto,
                                    '' contrato
                                    , horario.descricao AS jornada
                                    , convert(varchar,empresa.codigo)+' | '+ empresa.nome as empresa  
                                    , convert(varchar,departamento.codigo)+' | '+ departamento.descricao AS departamento
                                    , convert(varchar,funcao.codigo) + ' | ' + funcao.descricao AS funcao
                                    , coalesce(convert(varchar,cwu.codigo) + ' | ' + cwu.nome, '') AS supervisor
                                    , coalesce(convert(varchar, pe.codigo) + ' | ' + pe.razaosocial, '') as PessoaSupervisor
                                    , convert(varchar,Alocacao.codigo) + ' | ' + Alocacao.descricao AS Alocacao
                                    , convert(varchar,TipoVinculo.codigo) + ' | ' + TipoVinculo.descricao AS TipoVinculo
                                    , Alocacao.Endereco AlocacaoEndereco
                                    , func.IdHorarioDinamico
                                    , func.CicloSequenciaIndice
                                    , convert(varchar,HDN.codigo) +' | '+HDN.descricao HorarioDinamico
                             FROM funcionario func 
                             LEFT JOIN empresa ON empresa.id = func.idempresa
                             LEFT JOIN departamento ON departamento.id = func.iddepartamento
                             LEFT JOIN horario ON horario.id = func.idhorario
                             LEFT JOIN funcao ON funcao.id = func.idFuncao
                             LEFT JOIN cw_usuario cwu ON cwu.id = func.idcw_usuario
                             LEFT JOIN pessoa pe ON pe.id = func.IdPessoaSupervisor
                             LEFT JOIN Alocacao ON alocacao.id = func.idAlocacao
                             LEFT JOIN TipoVinculo ON TipoVinculo.id = func.idTipoVinculo
                             LEFT JOIN HorarioDinamico HDN ON HDN.id = func.idhorariodinamico
                            WHERE func.id in ({0}) ORDER BY func.nome", _func);

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Modelo.Funcionario objFuncionario = new Modelo.Funcionario();
                    AuxSetInstance(dr, objFuncionario);
                    lista.Add(objFuncionario);
                }
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return lista;
        }

        public List<Modelo.Funcionario> getLista(int pempresa)
        {
            List<Modelo.Funcionario> lista = new List<Modelo.Funcionario>();

            SqlParameter[] parms = new SqlParameter[]
            {
                 new SqlParameter("@idempresa", SqlDbType.Int)
            };
            parms[0].Value = pempresa;

            string aux = @"SELECT   func.id,
                                    func.codigo,
                                    func.dscodigo,
                                    func.matricula,
                                    func.nome,
                                    func.codigofolha,
                                    func.idempresa,
                                    func.iddepartamento,
                                    func.idfuncao,
                                    func.idhorario,
                                    func.tipohorario,
                                    func.carteira,
                                    func.dataadmissao,
                                    func.datademissao,
                                    func.salario,
                                    func.funcionarioativo,
                                    func.DataInativacao,
                                    func.naoentrarbanco,
                                    func.naoentrarcompensacao,
                                    func.excluido,
                                    func.campoobservacao,
                                    func.incdata,
                                    func.inchora,
                                    func.incusuario,
                                    func.altdata,
                                    func.althora,
                                    func.altusuario,
                                    func.pis,
                                    func.senha,
                                    func.toleranciaentrada,
                                    func.toleranciasaida,
                                    func.quantidadetickets,
                                    func.tipotickets,
                                    func.CPF,
                                    func.Mob_Senha,
                                    func.idcw_usuario,
                                    func.utilizaregistrador,
                                    func.UtilizaAppPontofopag,
                                    func.UtilizaReconhecimentoFacialApp,
                                    func.UtilizaWebAppPontofopag,
                                    func.UtilizaReconhecimentoFacialWebApp,
                                    func.idIntegracao,
                                    func.IdPessoaSupervisor,
                                    func.TipoMaoObra,
                                    func.IdAlocacao,
                                    func.IdTipoVinculo,
                                    func.Email,
                                    func.IdIntegracaoPainel,
                                    func.RFID,
                                    '' foto,
                                    '' contrato
                                    , horario.descricao AS jornada
                                    , convert(varchar,empresa.codigo)+' | '+empresa.nome as empresa  
                                    , convert(varchar,departamento.codigo)+' | '+ departamento.descricao AS departamento
                                    , convert(varchar,funcao.codigo) + ' | ' + funcao.descricao AS funcao
                                    , coalesce(convert(varchar,cwu.codigo) + ' | ' + cwu.nome, '') AS supervisor          
                                    , coalesce(convert(varchar, pe.codigo) + ' | ' + pe.razaosocial, '') as PessoaSupervisor    
                                    , convert(varchar,Alocacao.codigo) + ' | ' + Alocacao.descricao AS Alocacao                   
                                    , convert(varchar,TipoVinculo.codigo) + ' | ' + TipoVinculo.descricao as TipoVinculo                 
                                    , func.IdHorarioDinamico
                                    , func.CicloSequenciaIndice
                                    , convert(varchar,HDN.codigo) +' | '+HDN.descricao HorarioDinamico
                             FROM funcionario func 
                             LEFT JOIN empresa ON empresa.id = func.idEmpresa
                             LEFT JOIN horario ON horario.id = func.idHorario
                             LEFT JOIN departamento ON departamento.id = func.iddepartamento
                             LEFT JOIN funcao ON funcao.id = func.idFuncao
                             LEFT JOIN cw_usuario cwu ON cwu.id = func.idcw_usuario
                             LEFT JOIN pessoa pe ON pe.id = func.IdPessoaSupervisor
                             LEFT JOIN Alocacao ON alocacao.id = func.idAlocacao
                             LEFT JOIN TipoVinculo ON TipoVinculo.id = func.idTipoVinculo
                             LEFT JOIN HorarioDinamico HDN ON HDN.id = func.idhorariodinamico
                         WHERE ISNULL(func.excluido,0)=0 AND ISNULL(func.funcionarioativo,0)=1 
                         AND func.idempresa = @idempresa";


            aux += PermissaoUsuarioFuncionario(UsuarioLogado, aux, null, "func.id", null);
            aux += " ORDER BY func.nome";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Modelo.Funcionario objFuncionario = new Modelo.Funcionario();
                    AuxSetInstance(dr, objFuncionario);
                    lista.Add(objFuncionario);
                }
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return lista;
        }

        public List<Modelo.Funcionario> getLista(int pempresa, int pdepartamento)
        {
            List<Modelo.Funcionario> lista = new List<Modelo.Funcionario>();

            SqlParameter[] parms = new SqlParameter[]
            {
                 new SqlParameter("@idempresa", SqlDbType.Int),
                 new SqlParameter("@iddepartamento", SqlDbType.Int)
            };
            parms[0].Value = pempresa;
            parms[1].Value = pdepartamento;

            string aux = @"SELECT   func.id,
                                    func.codigo,
                                    func.dscodigo,
                                    func.matricula,
                                    func.nome,
                                    func.codigofolha,
                                    func.idempresa,
                                    func.iddepartamento,
                                    func.idfuncao,
                                    func.idhorario,
                                    func.tipohorario,
                                    func.carteira,
                                    func.dataadmissao,
                                    func.datademissao,
                                    func.salario,
                                    func.funcionarioativo,
                                    func.DataInativacao,
                                    func.naoentrarbanco,
                                    func.naoentrarcompensacao,
                                    func.excluido,
                                    func.campoobservacao,
                                    func.incdata,
                                    func.inchora,
                                    func.incusuario,
                                    func.altdata,
                                    func.althora,
                                    func.altusuario,
                                    func.pis,
                                    func.senha,
                                    func.toleranciaentrada,
                                    func.toleranciasaida,
                                    func.quantidadetickets,
                                    func.tipotickets,
                                    func.CPF,
                                    func.Mob_Senha,
                                    func.idcw_usuario,
                                    func.utilizaregistrador,
                                    func.UtilizaAppPontofopag,
                                    func.UtilizaReconhecimentoFacialApp,
                                    func.UtilizaWebAppPontofopag,
                                    func.UtilizaReconhecimentoFacialWebApp,
                                    func.idIntegracao,
                                    func.IdPessoaSupervisor,
                                    func.TipoMaoObra,
                                    func.IdAlocacao,
                                    func.IdTipoVinculo,
                                    func.Email,
                                    func.IdIntegracaoPainel,
                                    func.RFID,        
                                    '' foto,
                                    '' contrato
                                    , horario.descricao AS jornada
                                    , convert(varchar,empresa.codigo)+' | '+empresa.nome as empresa
                                    , coalesce(convert(varchar,cwu.codigo) + ' | ' + cwu.nome, '') AS supervisor
                                    , coalesce(convert(varchar, pe.codigo) + ' | ' + pe.razaosocial, '') as PessoaSupervisor
                                    , convert(varchar,Alocacao.codigo) + ' | ' + Alocacao.descricao AS Alocacao
                                    , convert(varchar,TipoVinculo.codigo) + ' | ' + TipoVinculo.descricao AS TipoVinculo
                                    , func.IdHorarioDinamico
                                    , func.CicloSequenciaIndice
                                    , convert(varchar,HDN.codigo) +' | '+HDN.descricao HorarioDinamico
                             FROM funcionario func 
                             LEFT JOIN empresa ON empresa.id = func.idEmpresa
                             LEFT JOIN horario ON horario.id = func.idHorario
                             LEFT JOIN cw_usuario cwu ON cwu.id = func.idcw_usuario
                             LEFT JOIN pessoa pe ON pe.id = func.IdPessoaSupervisor
                             LEFT JOIN Alocacao ON alocacao.id = func.idAlocacao
                             LEFT JOIN TipoVinculo ON TipoVinculo.id = func.idTipoVinculo
                             LEFT JOIN HorarioDinamico HDN ON HDN.id = func.idhorariodinamico
                         WHERE ISNULL(func.excluido,0)=0 AND ISNULL(func.funcionarioativo,0)=1 
                         AND func.idempresa = @idempresa
                         AND func.iddepartamento = @iddepartamento
                         ORDER BY funcionario.nome";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Modelo.Funcionario objFuncionario = new Modelo.Funcionario();
                    AuxSetInstance(dr, objFuncionario);

                    lista.Add(objFuncionario);
                }
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return lista;
        }

        public Modelo.Funcionario RetornaFuncDsCodigo(string pCodigo)
        {
            SqlParameter[] parms = new SqlParameter[1]
            {
                  new SqlParameter("@dscodigo", SqlDbType.VarChar)
            };
            parms[0].Value = pCodigo;

            string aux = @"SELECT     func.id,
                                    func.codigo,
                                    func.dscodigo,
                                    func.matricula,
                                    func.nome,
                                    func.codigofolha,
                                    func.idempresa,
                                    func.iddepartamento,
                                    func.idfuncao,
                                    func.idhorario,
                                    func.tipohorario,
                                    func.carteira,
                                    func.dataadmissao,
                                    func.datademissao,
                                    func.salario,
                                    func.funcionarioativo,
                                    func.DataInativacao,
                                    func.naoentrarbanco,
                                    func.naoentrarcompensacao,
                                    func.excluido,
                                    func.campoobservacao,
                                    func.incdata,
                                    func.inchora,
                                    func.incusuario,
                                    func.altdata,
                                    func.althora,
                                    func.altusuario,
                                    func.pis,
                                    func.senha,
                                    func.toleranciaentrada,
                                    func.toleranciasaida,
                                    func.quantidadetickets,
                                    func.tipotickets,
                                    func.CPF,
                                    func.Mob_Senha,
                                    func.idcw_usuario,
                                    func.utilizaregistrador,
                                    func.UtilizaAppPontofopag,
                                    func.UtilizaReconhecimentoFacialApp,
                                    func.UtilizaWebAppPontofopag,
                                    func.UtilizaReconhecimentoFacialWebApp,
                                    func.idIntegracao,
                                    func.IdPessoaSupervisor,
                                    func.TipoMaoObra,
                                    func.IdAlocacao,
                                    func.IdTipoVinculo,
                                    func.Email,
                                    func.IdIntegracaoPainel,
                                    func.RFID,
                                    '' foto, 
                                    '' contrato
                                    , horario.descricao AS jornada
                                    , convert(varchar,empresa.codigo)+' | '+empresa.nome as empresa
                                    , convert(varchar,departamento.codigo)+' | '+departamento.descricao AS departamento
                                    , convert(varchar,funcao.codigo) + ' | ' + funcao.descricao AS funcao
                                    , coalesce(convert(varchar,cwu.codigo) + ' | ' + cwu.nome, '') AS supervisor
                                    , coalesce(convert(varchar, pe.codigo) + ' | ' + pe.razaosocial, '') as PessoaSupervisor
                                    , convert(varchar,Alocacao.codigo) + ' | ' + Alocacao.descricao AS Alocacao
                                    , convert(varchar,TipoVinculo.codigo) + ' | ' + TipoVinculo.descricao AS TipoVinculo
                                    , func.IdHorarioDinamico
                                    , func.CicloSequenciaIndice
                                    , convert(varchar,HDN.codigo) +' | '+HDN.descricao HorarioDinamico
                             FROM funcionario func 
                             LEFT JOIN empresa ON empresa.id = func.idempresa
                             LEFT JOIN departamento ON departamento.id = func.iddepartamento
                             LEFT JOIN funcao ON funcao.id = func.idfuncao
                             LEFT JOIN cw_usuario cwu ON cwu.id = func.idcw_usuario
                             LEFT JOIN horario ON horario.id = func.idhorario
                             LEFT JOIN pessoa pe ON pe.id = func.IdPessoaSupervisor
                             LEFT JOIN Alocacao ON alocacao.id = func.idAlocacao
                             LEFT JOIN TipoVinculo ON TipoVinculo.id = func.idTipoVinculo
                             LEFT JOIN HorarioDinamico HDN ON HDN.id = func.idhorariodinamico
                             WHERE func.dscodigo = @dscodigo";

            SqlDataReader dr;
            dr = db.ExecuteReader(CommandType.Text, aux, parms);
            Modelo.Funcionario objFuncionario = new Modelo.Funcionario();
            if (dr.Read())
            {
                AuxSetInstance(dr, objFuncionario);
            }
            else
            {
                objFuncionario = null;
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();
            return objFuncionario;
        }

        public List<Modelo.Funcionario> RetornaFuncDsCodigos(List<String> pCodigo)
        {
            SqlParameter[] parms = new SqlParameter[1]
            {
                  new SqlParameter("@dscodigo", SqlDbType.VarChar)
            };
            parms[0].Value = String.Join(",", pCodigo);

            string aux = @"SELECT     func.id,
                                   func.codigo,
                                   func.dscodigo,
                                   func.matricula,
                                   func.nome,
                                   func.codigofolha,
                                   func.idempresa,
                                   func.iddepartamento,
                                   func.idfuncao,
                                   func.idhorario,
                                   func.tipohorario,
                                   func.carteira,
                                   func.dataadmissao,
                                   func.datademissao,
                                   func.salario,
                                   func.funcionarioativo,
                                   func.DataInativacao,
                                   func.naoentrarbanco,
                                   func.naoentrarcompensacao,
                                   func.excluido,
                                   func.campoobservacao,
                                   func.incdata,
                                   func.inchora,
                                   func.incusuario,
                                   func.altdata,
                                   func.althora,
                                   func.altusuario,
                                   func.pis,
                                   func.senha,
                                   func.toleranciaentrada,
                                   func.toleranciasaida,
                                   func.quantidadetickets,
                                   func.tipotickets,
                                   func.CPF,
                                   func.Mob_Senha,
                                   func.idcw_usuario,
                                   func.utilizaregistrador,
                                   func.UtilizaAppPontofopag,
                                   func.UtilizaReconhecimentoFacialApp,
                                   func.UtilizaWebAppPontofopag,
                                   func.UtilizaReconhecimentoFacialWebApp,
                                   func.idIntegracao,
                                   func.IdPessoaSupervisor,
                                   func.TipoMaoObra,
                                   func.IdAlocacao,
                                   func.IdTipoVinculo,
                                   func.Email,
                                   func.IdIntegracaoPainel
                                    , horario.descricao AS jornada
                                    , convert(varchar,empresa.codigo)+' | '+empresa.nome as empresa
                                    , convert(varchar,departamento.codigo)+' | '+departamento.descricao AS departamento
                                    , convert(varchar,funcao.codigo) + ' | ' + funcao.descricao AS funcao
                                    , coalesce(convert(varchar,cwu.codigo) + ' | ' + cwu.nome, '') AS supervisor
                                    , coalesce(convert(varchar, pe.codigo) + ' | ' + pe.razaosocial, '') as PessoaSupervisor
                                    , convert(varchar,Alocacao.codigo) + ' | ' + Alocacao.descricao AS Alocacao
                                    , convert(varchar,TipoVinculo.codigo) + ' | ' + TipoVinculo.descricao AS TipoVinculo
                                    , func.IdHorarioDinamico
                                    , func.CicloSequenciaIndice
                                    , convert(varchar,HDN.codigo) +' | '+HDN.descricao HorarioDinamico
                             FROM funcionario func 
                             LEFT JOIN empresa ON empresa.id = func.idempresa
                             LEFT JOIN departamento ON departamento.id = func.iddepartamento
                             LEFT JOIN funcao ON funcao.id = func.idfuncao
                             LEFT JOIN cw_usuario cwu ON cwu.id = func.idcw_usuario
                             LEFT JOIN horario ON horario.id = func.idhorario
                             LEFT JOIN pessoa pe ON pe.id = func.IdPessoaSupervisor
                             LEFT JOIN Alocacao ON alocacao.id = func.idAlocacao
                             LEFT JOIN TipoVinculo ON TipoVinculo.id = func.idTipoVinculo
                             LEFT JOIN HorarioDinamico HDN ON HDN.id = func.idhorariodinamico
                             WHERE func.dscodigo in ( SELECT * FROM dbo.F_ClausulaIn(@dscodigo))";

            SqlDataReader dr;
            dr = db.ExecuteReader(CommandType.Text, aux, parms);
            List<Modelo.Funcionario> lista = new List<Modelo.Funcionario>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.Funcionario>();
                lista = AutoMapper.Mapper.Map<List<Modelo.Funcionario>>(dr);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (!dr.IsClosed)
                {
                    dr.Close();
                }
                dr.Dispose();
            }
            return lista;
        }

        public Modelo.Funcionario RetornaFuncPis(int idFuncionario, string pis)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                  new SqlParameter("@pis", SqlDbType.VarChar),
                  new SqlParameter("@id", SqlDbType.Int)
            };
            parms[0].Value = pis;
            parms[1].Value = idFuncionario;

            string aux = @"SELECT     func.id,
                                   func.codigo,
                                   func.dscodigo,
                                   func.matricula,
                                   func.nome,
                                   func.codigofolha,
                                   func.idempresa,
                                   func.iddepartamento,
                                   func.idfuncao,
                                   func.idhorario,
                                   func.tipohorario,
                                   func.carteira,
                                   func.dataadmissao,
                                   func.datademissao,
                                   func.salario,
                                   func.funcionarioativo,
                                   func.DataInativacao,
                                   func.naoentrarbanco,
                                   func.naoentrarcompensacao,
                                   func.excluido,
                                   func.campoobservacao,
                                   func.incdata,
                                   func.inchora,
                                   func.incusuario,
                                   func.altdata,
                                   func.althora,
                                   func.altusuario,
                                   func.pis,
                                   func.senha,
                                   func.toleranciaentrada,
                                   func.toleranciasaida,
                                   func.quantidadetickets,
                                   func.tipotickets,
                                   func.CPF,
                                   func.Mob_Senha,
                                   func.idcw_usuario,
                                   func.utilizaregistrador,
                                   func.UtilizaAppPontofopag,
                                   func.UtilizaReconhecimentoFacialApp,
                                   func.UtilizaWebAppPontofopag,
                                   func.UtilizaReconhecimentoFacialWebApp,
                                   func.idIntegracao,
                                   func.IdPessoaSupervisor,
                                   func.TipoMaoObra,
                                   func.IdAlocacao,
                                   func.IdTipoVinculo,
                                   func.Email,
                                   func.IdIntegracaoPainel,
                                   func.RFID, 
                                    '' foto,
                                    '' contrato
                                    , horario.descricao AS jornada
                                    , convert(varchar,empresa.codigo)+' | '+empresa.nome as empresa
                                    , departamento.descricao AS departamento
                                    , convert(varchar,funcao.codigo) + ' | ' + funcao.descricao AS funcao
                                    , coalesce(convert(varchar,cwu.codigo) + ' | ' + cwu.nome, '') AS supervisor
                                    , coalesce(convert(varchar, pe.codigo) + ' | ' + pe.razaosocial, '') as PessoaSupervisor
                                    , convert(varchar,Alocacao.codigo) + ' | ' + Alocacao.descricao AS Alocacao
                                    , convert(varchar,TipoVinculo.codigo) + ' | ' + TipoVinculo.descricao AS TipoVinculo
                                    , func.IdHorarioDinamico
                                    , func.CicloSequenciaIndice
                                    , convert(varchar,HDN.codigo) +' | '+HDN.descricao HorarioDinamico
                             FROM funcionario func 
                             LEFT JOIN empresa ON empresa.id = func.idempresa
                             LEFT JOIN departamento ON departamento.id = func.iddepartamento
                             LEFT JOIN horario ON horario.id = func.idhorario
                             LEFT JOIN funcao ON funcao.id = func.idfuncao
                             LEFT JOIN cw_usuario cwu ON cwu.id = func.idcw_usuario
                             LEFT JOIN pessoa pe ON pe.id = func.IdPessoaSupervisor
                             LEFT JOIN Alocacao ON alocacao.id = func.idAlocacao
                             LEFT JOIN TipoVinculo ON TipoVinculo.id = func.idTipoVinculo
                             LEFT JOIN HorarioDinamico HDN ON HDN.id = func.idhorariodinamico
                             WHERE func.pis = @pis AND func.id <> @id AND func.datademissao is NULL AND func.excluido <> 1";

            SqlDataReader dr;
            dr = db.ExecuteReader(CommandType.Text, aux, parms);
            Modelo.Funcionario objFuncionario = new Modelo.Funcionario();
            if (dr.Read())
            {
                AuxSetInstance(dr, objFuncionario);
            }
            else
            {
                objFuncionario = null;
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();
            return objFuncionario;
        }

        /// <summary>
        /// Método utilizado para carregar os funcionarios na tabela de marcações
        /// </summary>
        /// <param name="tipo">1-Empresa, 2-Departamento, 3-Todos</param>
        /// <param name="identificacao">id da empresa ou do departamento</param>
        /// <returns>Lista dos funcionários</returns>
        public List<Modelo.Funcionario> GetTabelaMarcacao(int tipo, int identificacao, string consultaNomeFuncionario)
        {
            List<Modelo.Funcionario> lista = new List<Modelo.Funcionario>();
            SqlDataReader dr = GetTabelaMarcacaoDR(tipo, identificacao, consultaNomeFuncionario);

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Modelo.Funcionario objFuncionario = new Modelo.Funcionario();
                    AuxSetInstance(dr, objFuncionario);

                    lista.Add(objFuncionario);
                }
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();
            return lista;
        }

        /// <summary>
        /// Retorna os "qtdRegistros" proximos funcionários de acordo com o fitro
        /// </summary>
        /// <param name="tipo">1: empresa; 2: Departamento; 3: funcionario; 5: Contrato
        /// <param name="identificacao">Id do registro de acordo com o tipo</param>
        /// <param name="qtdRegistros"> qtd de registros que deseja que o select retorne</param>
        /// <param name="nomeFuncionario">Nome do funcionário caso deseje que os dados seja retornado a partir de um funcionário</param>
        /// <param name="tipoOrdenacao">0: ascendente; 1: decrescente, a ordenação é baseada no nome do funcionário</param>
        /// <param name="proximoOuAnterior">0: proximo; 1: anterior</param>
        /// <returns>Retorna a quantidade de registros solicitados de acordo com os parâmetros</returns>
        public List<Modelo.Funcionario> GetProximoOuAnterior(int tipo, int identificacao, int qtdRegistros, string nomeFuncionario, int tipoOrdenacao, int proximoOuAnterior)
        {

            SqlParameter[] parms = new SqlParameter[2]
                {
                    new SqlParameter("@identificacao", SqlDbType.Int),
                    new SqlParameter("@nome", SqlDbType.VarChar)
                };

            parms[0].Value = identificacao;
            parms[1].Value = nomeFuncionario;
            string comando = @"  SELECT TOP " + qtdRegistros + @" 
	                                   func.id,
	                                   func.nome,
	                                   func.dscodigo   
                             FROM funcionario func
                            WHERE 1 = 1
                             ";

            switch (tipo)
            {
                case 1:
                    comando += " AND func.idempresa = @identificacao";
                    break;
                case 2:
                    comando += " AND func.iddepartamento = @identificacao";
                    break;
                case 3:
                    comando += " AND func.id > 0";
                    break;
                case 5:
                    comando += " AND func.id in (select cf.idfuncionario from contratofuncionario cf where cf.idcontrato = @identificacao and cf.excluido = 0)";
                    break;
            }

            comando += " AND func.funcionarioativo = 1 AND ISNULL(func.excluido, 0) = 0";
            comando += PermissaoUsuarioFuncionario(UsuarioLogado, comando, "func.idempresa", "func.id", null);
            if (!String.IsNullOrEmpty(nomeFuncionario))
            {
                if (proximoOuAnterior == 0)
                {
                    comando += " AND func.nome >= @nome ";
                }
                else
                {
                    comando += " AND func.nome <= @nome ";
                }

            }
            comando += " ORDER BY LOWER(func.nome) ";
            if (tipoOrdenacao == 1)
            {
                comando += " DESC ";
            }

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, comando, parms);
            List<Modelo.Funcionario> lista = new List<Modelo.Funcionario>();

            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.Funcionario>();
                lista = AutoMapper.Mapper.Map<List<Modelo.Funcionario>>(dr);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (!dr.IsClosed)
                {
                    dr.Close();
                }
                dr.Dispose();
            }
            return lista;
        }


        /// <summary>
        /// Busca no banco de dados uma lista de funcionarios por departamento
        /// </summary>
        /// <param name="pIDDepartamento"> ID do departamento </param>
        /// <returns></returns>
        //PAM - 11/12/20009
        public List<Modelo.Funcionario> GetPorDepartamentoList(int pIDDepartamento)
        {
            List<Modelo.Funcionario> lista = new List<Modelo.Funcionario>();

            SqlParameter[] parms = new SqlParameter[]
            {
                 new SqlParameter("@iddepartamento", SqlDbType.Int)
            };
            parms[0].Value = pIDDepartamento;

            string aux = @"SELECT  funcionario.id,
                                   funcionario.codigo,
                                   funcionario.dscodigo,
                                   funcionario.matricula,
                                   funcionario.nome,
                                   funcionario.codigofolha,
                                   funcionario.idempresa,
                                   funcionario.iddepartamento,
                                   funcionario.idfuncao,
                                   funcionario.idhorario,
                                   funcionario.tipohorario,
                                   funcionario.carteira,
                                   funcionario.dataadmissao,
                                   funcionario.datademissao,
                                   funcionario.salario,
                                   funcionario.funcionarioativo,
                                   funcionario.DataInativacao,
                                   funcionario.naoentrarbanco,
                                   funcionario.naoentrarcompensacao,
                                   funcionario.excluido,
                                   funcionario.campoobservacao,
                                   funcionario.incdata,
                                   funcionario.inchora,
                                   funcionario.incusuario,
                                   funcionario.altdata,
                                   funcionario.althora,
                                   funcionario.altusuario,
                                   funcionario.pis,
                                   funcionario.senha,
                                   funcionario.toleranciaentrada,
                                   funcionario.toleranciasaida,
                                   funcionario.quantidadetickets,
                                   funcionario.tipotickets,
                                   funcionario.CPF,
                                   funcionario.Mob_Senha,
                                   funcionario.idcw_usuario,
                                   funcionario.utilizaregistrador,
                                   funcionario.UtilizaAppPontofopag,
                                   funcionario.UtilizaReconhecimentoFacialApp,
                                   funcionario.UtilizaWebAppPontofopag,
                                   funcionario.UtilizaReconhecimentoFacialWebApp,
                                   funcionario.idIntegracao,
                                   funcionario.IdPessoaSupervisor,
                                   funcionario.TipoMaoObra,
                                   funcionario.IdAlocacao,
                                   funcionario.IdTipoVinculo,
                                   funcionario.Email,
                                   funcionario.IdIntegracaoPainel,
                                   funcionario.RFID, 
                                    '' foto,
                                    '' contrato
                         , horario.descricao AS jornada
                         , convert(varchar,empresa.codigo)+' | '+empresa.nome as empresa
                         , departamento.descricao AS departamento
                         , convert(varchar,funcao.codigo) + ' | ' + funcao.descricao AS funcao
                         , coalesce(convert(varchar,cwu.codigo) + ' | ' + cwu.nome, '') AS supervisor
                         , coalesce(convert(varchar, pe.codigo) + ' | ' + pe.razaosocial, '') as PessoaSupervisor
                         , convert(varchar,Alocacao.codigo) + ' | ' + Alocacao.descricao AS Alocacao
                         , convert(varchar,TipoVinculo.codigo) + ' | ' + TipoVinculo.descricao AS TipoVinculo
                         , funcionario.IdHorarioDinamico
                         , funcionario.CicloSequenciaIndice
                         , convert(varchar,HDN.codigo) +' | '+HDN.descricao HorarioDinamico
                         FROM funcionario
                         LEFT JOIN empresa ON empresa.id = funcionario.idempresa
                         LEFT JOIN departamento ON departamento.id = funcionario.iddepartamento
                         LEFT JOIN horario ON horario.id = funcionario.idhorario
                         LEFT JOIN funcao ON funcao.id = funcionario.idfuncao
                         LEFT JOIN cw_usuario cwu ON cwu.id = funcionario.idcw_usuario
                         LEFT JOIN pessoa pe ON pe.id = funcionario.IdPessoaSupervisor
                         LEFT JOIN Alocacao ON alocacao.id = funcionario.idAlocacao
                         LEFT JOIN TipoVinculo ON TipoVinculo.id = funcionario.idTipoVinculo
                         LEFT JOIN HorarioDinamico HDN ON HDN.id = funcionario.idhorariodinamico
                         WHERE ISNULL(funcionario.excluido,0)=0 AND ISNULL(funcionario.funcionarioativo,0)=1 
                         AND funcionario.iddepartamento = @iddepartamento
                         ORDER BY funcionario.nome";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Modelo.Funcionario objFuncionario = new Modelo.Funcionario();
                    AuxSetInstance(dr, objFuncionario);
                    lista.Add(objFuncionario);
                }
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return lista;
        }

        /// <summary>
        /// Pega os funcionarios de uma determinada função do banco de dados
        /// </summary>
        /// <param name="pIdFuncao"> Id da função </param>
        /// <returns>Lista de funcionarios que tem aquela funcao</returns>
        //PAM - 11/12/2009
        public List<Modelo.Funcionario> GetPorFuncaoList(int pIdFuncao)
        {
            List<Modelo.Funcionario> lista = new List<Modelo.Funcionario>();

            SqlParameter[] parms = new SqlParameter[]
            {
                 new SqlParameter("@idfuncao", SqlDbType.Int)
            };
            parms[0].Value = pIdFuncao;

            string aux = @"SELECT  funcionario.id,
                                   funcionario.codigo,
                                   funcionario.dscodigo,
                                   funcionario.matricula,
                                   funcionario.nome,
                                   funcionario.codigofolha,
                                   funcionario.idempresa,
                                   funcionario.iddepartamento,
                                   funcionario.idfuncao,
                                   funcionario.idhorario,
                                   funcionario.tipohorario,
                                   funcionario.carteira,
                                   funcionario.dataadmissao,
                                   funcionario.datademissao,
                                   funcionario.salario,
                                   funcionario.funcionarioativo,
                                   funcionario.DataInativacao,
                                   funcionario.naoentrarbanco,
                                   funcionario.naoentrarcompensacao,
                                   funcionario.excluido,
                                   funcionario.campoobservacao,
                                   funcionario.incdata,
                                   funcionario.inchora,
                                   funcionario.incusuario,
                                   funcionario.altdata,
                                   funcionario.althora,
                                   funcionario.altusuario,
                                   funcionario.pis,
                                   funcionario.senha,
                                   funcionario.toleranciaentrada,
                                   funcionario.toleranciasaida,
                                   funcionario.quantidadetickets,
                                   funcionario.tipotickets,
                                   funcionario.CPF,
                                   funcionario.Mob_Senha,
                                   funcionario.idcw_usuario,
                                   funcionario.utilizaregistrador,
                                   funcionario.UtilizaAppPontofopag,
                                   funcionario.UtilizaReconhecimentoFacialApp,
                                   funcionario.UtilizaWebAppPontofopag,
                                   funcionario.UtilizaReconhecimentoFacialWebApp,
                                   funcionario.idIntegracao,
                                   funcionario.IdPessoaSupervisor,
                                   funcionario.TipoMaoObra,
                                   funcionario.IdAlocacao,
                                   funcionario.IdTipoVinculo,
                                   funcionario.Email,
                                   funcionario.IdIntegracaoPainel,
                                   funcionario.RFID, 
                                    '' foto,
                                    '' contrato
                         , horario.descricao AS jornada
                         , convert(varchar,empresa.codigo)+' | '+empresa.nome as empresa
                         , departamento.descricao AS departamento
                         , convert(varchar,funcao.codigo) + ' | ' + funcao.descricao AS funcao
                         , coalesce(convert(varchar,cwu.codigo) + ' | ' + cwu.nome, '') AS supervisor
                         , coalesce(convert(varchar, pe.codigo) + ' | ' + pe.razaosocial, '') as PessoaSupervisor
                         , convert(varchar,Alocacao.codigo) + ' | ' + Alocacao.descricao AS Alocacao
                         , convert(varchar,TipoVinculo.codigo) + ' | ' + TipoVinculo.descricao AS TipoVinculo
                         , funcionario.IdHorarioDinamico
                         , funcionario.CicloSequenciaIndice
                         , convert(varchar,HDN.codigo) +' | '+HDN.descricao HorarioDinamico
                         FROM funcionario
                         LEFT JOIN empresa ON empresa.id = funcionario.idempresa
                         LEFT JOIN departamento ON departamento.id = funcionario.iddepartamento
                         LEFT JOIN horario ON horario.id = funcionario.idhorario
                         LEFT JOIN funcao ON funcao.id = funcionario.idfuncao
                         LEFT JOIN cw_usuario cwu ON cwu.id = funcionario.idcw_usuario
                         LEFT JOIN pessoa pe ON pe.id = funcionario.IdPessoaSupervisor
                         LEFT JOIN Alocacao ON alocacao.id = funcionario.idAlocacao
                         LEFT JOIN TipoVinculo ON TipoVinculo.id = funcionario.idTipoVinculo
                         LEFT JOIN HorarioDinamico HDN ON HDN.id = funcionario.idhorariodinamico
                         WHERE ISNULL(funcionario.excluido,0) = 0 AND ISNULL(funcionario.funcionarioativo, 0) = 1 
                         AND funcionario.idfuncao = @idfuncao";

            //if (Modelo.cwkGlobal.objUsuarioLogado.Tipo != 0)
            //{
            //    aux += " AND (SELECT COUNT(id) FROM empresausuario WHERE empresausuario.idusuario = "
            //        + Modelo.cwkGlobal.objUsuarioLogado.Id.ToString() + " AND empresausuario.idempresa = funcionario.idempresa) > 0";
            //}
            aux += " ORDER BY funcionario.nome";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Modelo.Funcionario objFuncionario = new Modelo.Funcionario();
                    AuxSetInstance(dr, objFuncionario);
                    lista.Add(objFuncionario);
                }
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return lista;
        }
        public List<Modelo.Funcionario> GetPorAlocacao(int pidAlocacao)
        {
            List<Modelo.Funcionario> lista = new List<Modelo.Funcionario>();

            SqlParameter[] parms = new SqlParameter[]
            {
                 new SqlParameter("@IdAlocacao", SqlDbType.Int)
            };
            parms[0].Value = pidAlocacao;

            string aux = @"SELECT  funcionario.id,
                                   funcionario.codigo,
                                   funcionario.dscodigo,
                                   funcionario.matricula,
                                   funcionario.nome,
                                   funcionario.codigofolha,
                                   funcionario.idempresa,
                                   funcionario.iddepartamento,
                                   funcionario.idfuncao,
                                   funcionario.idhorario,
                                   funcionario.tipohorario,
                                   funcionario.carteira,
                                   funcionario.dataadmissao,
                                   funcionario.datademissao,
                                   funcionario.salario,
                                   funcionario.funcionarioativo,
                                   funcionario.DataInativacao,
                                   funcionario.naoentrarbanco,
                                   funcionario.naoentrarcompensacao,
                                   funcionario.excluido,
                                   funcionario.campoobservacao,
                                   funcionario.incdata,
                                   funcionario.inchora,
                                   funcionario.incusuario,
                                   funcionario.altdata,
                                   funcionario.althora,
                                   funcionario.altusuario,
                                   funcionario.pis,
                                   funcionario.senha,
                                   funcionario.toleranciaentrada,
                                   funcionario.toleranciasaida,
                                   funcionario.quantidadetickets,
                                   funcionario.tipotickets,
                                   funcionario.CPF,
                                   funcionario.Mob_Senha,
                                   funcionario.idcw_usuario,
                                   funcionario.utilizaregistrador,
                                   funcionario.UtilizaAppPontofopag,
                                   funcionario.UtilizaReconhecimentoFacialApp,
                                   funcionario.UtilizaWebAppPontofopag,
                                   funcionario.UtilizaReconhecimentoFacialWebApp,
                                   funcionario.idIntegracao,
                                   funcionario.IdPessoaSupervisor,
                                   funcionario.TipoMaoObra,
                                   funcionario.IdAlocacao,
                                   funcionario.IdTipoVinculo,
                                   funcionario.Email,
                                   funcionario.IdIntegracaoPainel,
                                   funcionario.RFID, 
                                    '' foto,
                                    '' contrato
                         , horario.descricao AS jornada
                         , convert(varchar,empresa.codigo)+' | '+empresa.nome as empresa
                         , departamento.descricao AS departamento
                         , convert(varchar,funcao.codigo) + ' | ' + funcao.descricao AS funcao
                         , coalesce(convert(varchar,cwu.codigo) + ' | ' + cwu.nome, '') AS supervisor
                         , coalesce(convert(varchar, pe.codigo) + ' | ' + pe.razaosocial, '') as PessoaSupervisor
                         , convert(varchar,Alocacao.codigo) + ' | ' + Alocacao.descricao AS Alocacao
                         , convert(varchar,TipoVinculo.codigo) + ' | ' + TipoVinculo.descricao AS TipoVinculo
                         , funcionario.IdHorarioDinamico
                         , funcionario.CicloSequenciaIndice
                         , convert(varchar,HDN.codigo) +' | '+HDN.descricao HorarioDinamico
                         FROM funcionario
                         LEFT JOIN empresa ON empresa.id = funcionario.idempresa
                         LEFT JOIN departamento ON departamento.id = funcionario.iddepartamento
                         LEFT JOIN horario ON horario.id = funcionario.idhorario
                         LEFT JOIN funcao ON funcao.id = funcionario.idfuncao
                         LEFT JOIN cw_usuario cwu ON cwu.id = funcionario.idcw_usuario
                         LEFT JOIN pessoa pe ON pe.id = funcionario.IdPessoaSupervisor
                         LEFT JOIN Alocacao ON alocacao.id = funcionario.idAlocacao
                         LEFT JOIN TipoVinculo ON TipoVinculo.id = funcionario.idTipoVinculo
                         LEFT JOIN HorarioDinamico HDN ON HDN.id = funcionario.idhorariodinamico
                         WHERE ISNULL(funcionario.excluido,0) = 0 AND ISNULL(funcionario.funcionarioativo, 0) = 1 
                         AND funcionario.idfuncao = @idfuncao";

            //if (Modelo.cwkGlobal.objUsuarioLogado.Tipo != 0)
            //{
            //    aux += " AND (SELECT COUNT(id) FROM empresausuario WHERE empresausuario.idusuario = "
            //        + Modelo.cwkGlobal.objUsuarioLogado.Id.ToString() + " AND empresausuario.idempresa = funcionario.idempresa) > 0";
            //}
            aux += " ORDER BY funcionario.nome";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Modelo.Funcionario objFuncionario = new Modelo.Funcionario();
                    AuxSetInstance(dr, objFuncionario);
                    lista.Add(objFuncionario);
                }
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return lista;
        }

        /// <summary>
        /// Pega todos os funcionarios que tem aquele horario
        /// </summary>
        /// <param name="pIdHorario">Id do horário</param>
        /// <returns>Lista (List) de funcionarios daquele horario</returns>
        //PAM 05/04/2010
        public List<Modelo.Funcionario> GetPorHorario(int pIdHorario)
        {
            List<Modelo.Funcionario> lista = new List<Modelo.Funcionario>();

            SqlParameter[] parms = new SqlParameter[]
            {
                 new SqlParameter("@idhorario", SqlDbType.Int)
            };
            parms[0].Value = pIdHorario;

            string aux = @" SELECT   func.id,
                                   func.codigo,
                                   func.dscodigo,
                                   func.matricula,
                                   func.nome,
                                   func.codigofolha,
                                   func.idempresa,
                                   func.iddepartamento,
                                   func.idfuncao,
                                   func.idhorario,
                                   func.tipohorario,
                                   func.carteira,
                                   func.dataadmissao,
                                   func.datademissao,
                                   func.salario,
                                   func.funcionarioativo,
                                   func.DataInativacao,
                                   func.naoentrarbanco,
                                   func.naoentrarcompensacao,
                                   func.excluido,
                                   func.campoobservacao,
                                   func.incdata,
                                   func.inchora,
                                   func.incusuario,
                                   func.altdata,
                                   func.althora,
                                   func.altusuario,
                                   func.pis,
                                   func.senha,
                                   func.toleranciaentrada,
                                   func.toleranciasaida,
                                   func.quantidadetickets,
                                   func.tipotickets,
                                   func.CPF,
                                   func.Mob_Senha,
                                   func.idcw_usuario,
                                   func.utilizaregistrador,
                                   func.UtilizaAppPontofopag,
                                   func.UtilizaReconhecimentoFacialApp,
                                   func.UtilizaWebAppPontofopag,
                                   func.UtilizaReconhecimentoFacialWebApp,
                                   func.idIntegracao,
                                   func.IdPessoaSupervisor,
                                   func.TipoMaoObra,
                                   func.IdAlocacao,
                                   func.IdTipoVinculo,
                                   func.Email,
                                   func.IdIntegracaoPainel,
                                   func.RFID, 
                                    '' foto,
                                    '' contrato
                                    , horario.descricao AS jornada
                                    , convert(varchar,empresa.codigo)+' | '+empresa.nome as empresa
                                    , convert(varchar,departamento.codigo)+' | '+departamento.descricao AS departamento
                                    , convert(varchar,funcao.codigo) + ' | ' + funcao.descricao AS funcao
                                    , coalesce(convert(varchar,cwu.codigo) + ' | ' + cwu.nome, '') AS supervisor
                                    , coalesce(convert(varchar, pe.codigo) + ' | ' + pe.razaosocial, '') as PessoaSupervisor
                                    , convert(varchar,Alocacao.codigo) + ' | ' + Alocacao.descricao AS Alocacao
                                    , convert(varchar,TipoVinculo.codigo) + ' | ' + TipoVinculo.descricao AS TipoVinculo
                                    , func.IdHorarioDinamico
                                    , func.CicloSequenciaIndice
                                    , convert(varchar,HDN.codigo) +' | '+HDN.descricao HorarioDinamico
                             FROM funcionario func 
                             LEFT JOIN empresa ON empresa.id = func.idempresa
                             LEFT JOIN departamento ON departamento.id = func.iddepartamento
                             LEFT JOIN funcao ON funcao.id = func.idFuncao
                             LEFT JOIN cw_usuario cwu ON cwu.id = func.idcw_usuario
                             LEFT JOIN pessoa pe ON pe.id = func.IdPessoaSupervisor
                             LEFT JOIN horario ON horario.id = func.idhorario
                             LEFT JOIN Alocacao ON alocacao.id = func.idAlocacao
                             LEFT JOIN TipoVinculo ON TipoVinculo.id = func.idTipoVinculo
                             LEFT JOIN HorarioDinamico HDN ON HDN.id = func.idhorariodinamico
							 WHERE ISNULL(func.excluido,0) = 0 AND ISNULL(func.funcionarioativo, 0) = 1 
							AND func.idhorario = @idhorario ";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Modelo.Funcionario objFuncionario = new Modelo.Funcionario();
                    AuxSetInstance(dr, objFuncionario);
                    lista.Add(objFuncionario);
                }
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return lista;
        }

        public Hashtable GetHashCodigoId()
        {
            Hashtable lista = new Hashtable();

            string aux = "SELECT codigo, id FROM funcionario";

            //if (Modelo.cwkGlobal.objUsuarioLogado.Tipo != 0)
            //{
            //    aux += " WHERE (SELECT COUNT(id) FROM empresausuario WHERE empresausuario.idusuario = "
            //        + Modelo.cwkGlobal.objUsuarioLogado.Id.ToString() + " AND empresausuario.idempresa = funcionario.idempresa) > 0";
            //}

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, new SqlParameter[] { });

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    lista.Add(Convert.ToInt32(dr["codigo"]), Convert.ToInt32(dr["id"]));
                }
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return lista;
        }

        public Hashtable GetHashCodigoFunc()
        {
            Hashtable lista = new Hashtable();

            string aux = @"   SELECT   func.id,
                                       func.codigo,
                                       func.dscodigo,
                                       func.matricula,
                                       func.nome,
                                       func.codigofolha,
                                       func.idempresa,
                                       func.iddepartamento,
                                       func.idfuncao,
                                       func.idhorario,
                                       func.tipohorario,
                                       func.carteira,
                                       func.dataadmissao,
                                       func.datademissao,
                                       func.salario,
                                       func.funcionarioativo,
                                       func.DataInativacao,
                                       func.naoentrarbanco,
                                       func.naoentrarcompensacao,
                                       func.excluido,
                                       func.campoobservacao,
                                       func.incdata,
                                       func.inchora,
                                       func.incusuario,
                                       func.altdata,
                                       func.althora,
                                       func.altusuario,
                                       func.pis,
                                       func.senha,
                                       func.toleranciaentrada,
                                       func.toleranciasaida,
                                       func.quantidadetickets,
                                       func.tipotickets,
                                       func.CPF,
                                       func.Mob_Senha,
                                       func.idcw_usuario,
                                       func.utilizaregistrador,
                                       func.UtilizaAppPontofopag,
                                       func.UtilizaReconhecimentoFacialApp,
                                       func.UtilizaWebAppPontofopag,
                                       func.UtilizaReconhecimentoFacialWebApp,
                                       func.idIntegracao,
                                       func.IdPessoaSupervisor,
                                       func.TipoMaoObra,
                                       func.IdAlocacao,
                                       func.IdTipoVinculo,
                                       func.Email,
                                       func.IdIntegracaoPainel,
                                       func.RFID, 
                                        '' foto,
                                       '' contrato
                                    , horario.descricao AS jornada
                                    , convert(varchar,empresa.codigo)+' | '+empresa.nome as empresa
                                    , convert(varchar,departamento.codigo)+' | '+departamento.descricao AS departamento
                                    , convert(varchar,funcao.codigo) + ' | ' + funcao.descricao AS funcao
                                    , coalesce(convert(varchar,cwu.codigo) + ' | ' + cwu.nome, '') AS supervisor
                                    , coalesce(convert(varchar, pe.codigo) + ' | ' + pe.razaosocial, '') as PessoaSupervisor
                                    , convert(varchar,Alocacao.codigo) + ' | ' + Alocacao.descricao AS Alocacao
                                    , convert(varchar,TipoVinculo.codigo) + ' | ' + TipoVinculo.descricao AS TipoVinculo
                                    , func.IdHorarioDinamico
                                    , func.CicloSequenciaIndice
                                    , convert(varchar,HDN.codigo) +' | '+HDN.descricao HorarioDinamico
                             FROM funcionario func 
                             LEFT JOIN empresa ON empresa.id = func.idempresa
                             LEFT JOIN departamento ON departamento.id = func.iddepartamento
                             LEFT JOIN funcao ON funcao.id = func.idFuncao
                             LEFT JOIN cw_usuario cwu ON cwu.id = func.idcw_usuario
                             LEFT JOIN pessoa pe ON pe.id = func.IdPessoaSupervisor
                             LEFT JOIN horario ON horario.id = func.idhorario
                             LEFT JOIN Alocacao ON alocacao.id = func.idAlocacao
                             LEFT JOIN TipoVinculo ON TipoVinculo.id = func.idTipoVinculo
                             LEFT JOIN HorarioDinamico HDN ON HDN.id = func.idhorariodinamico ";

            //if (Modelo.cwkGlobal.objUsuarioLogado.Tipo != 0)
            //{
            //    aux += " WHERE (SELECT COUNT(id) FROM empresausuario WHERE empresausuario.idusuario = "
            //        + Modelo.cwkGlobal.objUsuarioLogado.Id.ToString() + " AND empresausuario.idempresa = func.idempresa) > 0";
            //}

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, new SqlParameter[] { });
            Modelo.Funcionario obj = null;
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    obj = new Modelo.Funcionario();
                    AuxSetInstance(dr, obj);
                    lista.Add(Convert.ToInt32(dr["codigo"]), obj);
                }
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return lista;
        }

        public Hashtable GetHashIdFunc()
        {
            Hashtable lista = new Hashtable();

            string aux = @"   SELECT   func.id,
                                       func.codigo,
                                       func.dscodigo,
                                       func.matricula,
                                       func.nome,
                                       func.codigofolha,
                                       func.idempresa,
                                       func.iddepartamento,
                                       func.idfuncao,
                                       func.idhorario,
                                       func.tipohorario,
                                       func.carteira,
                                       func.dataadmissao,
                                       func.datademissao,
                                       func.salario,
                                       func.funcionarioativo,
                                       func.DataInativacao,
                                       func.naoentrarbanco,
                                       func.naoentrarcompensacao,
                                       func.excluido,
                                       func.campoobservacao,
                                       func.incdata,
                                       func.inchora,
                                       func.incusuario,
                                       func.altdata,
                                       func.althora,
                                       func.altusuario,
                                       func.pis,
                                       func.senha,
                                       func.toleranciaentrada,
                                       func.toleranciasaida,
                                       func.quantidadetickets,
                                       func.tipotickets,
                                       func.CPF,
                                       func.Mob_Senha,
                                       func.idcw_usuario,
                                       func.utilizaregistrador,
                                       func.UtilizaAppPontofopag,
                                       func.UtilizaReconhecimentoFacialApp,
                                       func.UtilizaWebAppPontofopag,
                                       func.UtilizaReconhecimentoFacialWebApp,
                                       func.idIntegracao,
                                       func.IdPessoaSupervisor,
                                       func.TipoMaoObra,
                                       func.IdAlocacao,
                                       func.IdTipoVinculo,
                                       func.Email,
                                       func.IdIntegracaoPainel,
                                       func.RFID, 
                                        '' foto,
                                        '' contrato
                                    , horario.descricao AS jornada
                                    , convert(varchar,empresa.codigo)+' | '+empresa.nome as empresa
                                    , convert(varchar,departamento.codigo)+' | '+departamento.descricao AS departamento
                                    , convert(varchar,funcao.codigo) + ' | ' + funcao.descricao AS funcao
                                    , coalesce(convert(varchar,cwu.codigo) + ' | ' + cwu.nome, '') AS supervisor
                                    , coalesce(convert(varchar, pe.codigo) + ' | ' + pe.razaosocial, '') as PessoaSupervisor
                                    , convert(varchar,Alocacao.codigo) + ' | ' + Alocacao.descricao AS Alocacao
                                    , convert(varchar,TipoVinculo.codigo) + ' | ' + TipoVinculo.descricao AS TipoVinculo
                                    , func.IdHorarioDinamico
                                    , func.CicloSequenciaIndice
                                    , convert(varchar,HDN.codigo) +' | '+HDN.descricao HorarioDinamico
                             FROM funcionario func 
                             LEFT JOIN empresa ON empresa.id = func.idempresa
                             LEFT JOIN departamento ON departamento.id = func.iddepartamento
                             LEFT JOIN funcao ON funcao.id = func.idFuncao
                             LEFT JOIN cw_usuario cwu ON cwu.id = func.idcw_usuario
                             LEFT JOIN pessoa pe ON pe.id = func.IdPessoaSupervisor
                             LEFT JOIN Alocacao ON alocacao.id = func.idAlocacao
                             LEFT JOIN horario ON horario.id = func.idhorario
                             LEFT JOIN TipoVinculo ON TipoVinculo.id = func.idTipoVinculo
                             LEFT JOIN HorarioDinamico HDN ON HDN.id = func.idhorariodinamico ";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, new SqlParameter[] { });
            Modelo.Funcionario obj = null;
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    obj = new Modelo.Funcionario();
                    AuxSetInstance(dr, obj);
                    lista.Add(Convert.ToInt32(dr["id"]), obj);
                }
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return lista;
        }

        public string GetDsCodigo(string pPis)
        {
            string aux;
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter ("@pis", SqlDbType.VarChar, 20)

            };
            parms[0].Value = pPis;
            if (pPis != null && pPis != "")
            {
                aux = "SELECT dscodigo FROM funcionario WHERE pis = @pis";
                return (string)db.ExecuteScalar(CommandType.Text, aux, parms);
            }
            else
            {
                return null;
            }

        }

        public DataTable GetPisCodigo(bool webApi)
        {
            DataTable dt = new DataTable();

            string aux = @"   SELECT  func.dscodigo
                                    , (case when LEN(func.pis) = 11 then '0' + func.pis else func.pis end) AS pis
                                    , func.excluido
                                    , func.funcionarioativo
                             FROM funcionario func
                             INNER JOIN empresa emp ON emp.id = func.idempresa
                             WHERE LEN(func.pis) BETWEEN 11 AND 12 "
                         + "  ORDER BY func.excluido, ISNULL(func.datademissao, DATEADD(YEAR, 1000, GETDATE())) DESC, ISNULL(func.althora, DATEADD(YEAR, 1000, GETDATE())) DESC";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, null);
            dt.Load(dr);
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return dt;
        }
        public DataTable GetPisCodigo()
        {
            return GetPisCodigo(new List<string> ());
        }
        public DataTable GetPisCodigo(List<string> pis)
        {
            DataTable dt = new DataTable();

            string aux = @"   SELECT  func.dscodigo
                                    , (case when LEN(func.pis) = 11 then '0' + func.pis else func.pis end) AS pis
                                    , func.excluido
                                    , func.funcionarioativo
                                    , func.id
                             FROM funcionario func
                             INNER JOIN empresa emp ON emp.id = func.idempresa
                             WHERE LEN(func.pis) BETWEEN 11 AND 12 ";

            //aux += PermissaoUsuarioFuncionario(UsuarioLogado, aux, "func.idempresa", "func.id", null);

            if (pis != null && pis.Count() > 0)
            {
                aux += " and func.pis in ('"+String.Join("','", pis) +"') ";
            }

            aux += "  ORDER BY func.excluido, ISNULL(func.datademissao, DATEADD(YEAR, 1000, GETDATE())) DESC, ISNULL(func.althora, DATEADD(YEAR, 1000, GETDATE())) DESC";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, null);
            dt.Load(dr);
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return dt;
        }
        #endregion

        #endregion

        public DataTable GetPorEmpresa(string pEmpresas)
        {
            DataTable dt = new DataTable();
            string aux = @"   SELECT   func.id
                                    , CAST(func.dscodigo AS BIGINT) AS codigo 
                                    , func.pis
                                    , func.nome
                                    , func.funcionarioativo
                                    , func.DataInativacao
                                    , func.matricula AS matricula
                                    , ISNULL (func.dataadmissao,' ') AS dataadmissao
                                    , ISNULL (func.datademissao,' ') AS datademissao
                                    , convert(varchar,emp.codigo)+' | '+emp.nome as empresa
                                    , convert(varchar,dep.codigo)+' | '+dep.descricao AS departamento
                                    , convert(varchar,funcao.codigo) + ' | ' + funcao.descricao AS funcao
                                    , coalesce(convert(varchar,cwu.codigo) + ' | ' + cwu.nome, '') AS supervisor
                                    , coalesce(convert(varchar, pe.codigo) + ' | ' + pe.razaosocial, '') as PessoaSupervisor
                                    , func.CPF
                                    , func.Mob_Senha
                                    , hor.descricao AS horario
                                    , (SELECT TOP 1 ISNULL(hd.entrada_1, '--:--') + ' - ' + ISNULL(hd.saida_1, '--:--') + ' | ' + ISNULL(hd.entrada_2, '--:--') + ' - ' + ISNULL(hd.saida_2, '--:--') 
                                    + ' | ' + ISNULL(hd.entrada_3, '--:--') + ' - ' + ISNULL(hd.saida_3, '--:--') + ' | ' + ISNULL(hd.entrada_4, '--:--') + ' - ' + ISNULL(hd.saida_4, '--:--')
                                      FROM horariodetalhe hd WHERE hd.idhorario = hor.id AND (hd.dia = 1 OR DATEPART(WEEKDAY, hd.data) = 2)) AS horariodescricao
                             FROM funcionario func
                             LEFT JOIN empresa emp ON emp.id = func.idempresa
                             LEFT JOIN departamento dep ON dep.id = func.iddepartamento
                             LEFT JOIN horario hor ON hor.id = func.idhorario
                             LEFT JOIN funcao ON funcao.id = func.idFuncao
                             LEFT JOIN cw_usuario cwu ON cwu.id = func.idcw_usuario
                             LEFT JOIN pessoa pe ON pe.id = func.IdPessoaSupervisor
                             WHERE ISNULL(func.excluido, 0) = 0 AND func.IdEmpresa IN " + pEmpresas;
            aux += " ORDER BY departamento, func.nome";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, null);
            dt.Load(dr);
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return dt;
        }

        public IList<Modelo.Funcionario> GetPorEmpresaList(string pEmpresas)
        {
            List<Modelo.Funcionario> lista = new List<Modelo.Funcionario>();

            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@empresas", SqlDbType.VarChar)
            };
            parms[0].Value = pEmpresas;

            string sql = @"SELECT   func.id,
                                   func.codigo,
                                   func.dscodigo,
                                   func.matricula,
                                   func.nome,
                                   func.codigofolha,
                                   func.idempresa,
                                   func.iddepartamento,
                                   func.idfuncao,
                                   func.idhorario,
                                   func.tipohorario,
                                   func.carteira,
                                   func.dataadmissao,
                                   func.datademissao,
                                   func.salario,
                                   func.funcionarioativo,
                                   func.DataInativacao,
                                   func.naoentrarbanco,
                                   func.naoentrarcompensacao,
                                   func.excluido,
                                   func.campoobservacao,
                                   func.incdata,
                                   func.inchora,
                                   func.incusuario,
                                   func.altdata,
                                   func.althora,
                                   func.altusuario,
                                   func.pis,
                                   func.senha,
                                   func.toleranciaentrada,
                                   func.toleranciasaida,
                                   func.quantidadetickets,
                                   func.tipotickets,
                                   func.CPF,
                                   func.Mob_Senha,
                                   func.idcw_usuario,
                                   func.utilizaregistrador,
                                   func.UtilizaAppPontofopag,
                                   func.UtilizaReconhecimentoFacialApp,
                                   func.UtilizaWebAppPontofopag,
                                   func.UtilizaReconhecimentoFacialWebApp,
                                   func.idIntegracao,
                                   func.IdPessoaSupervisor,
                                   func.TipoMaoObra,
                                   func.IdAlocacao,
                                   func.IdTipoVinculo,
                                   func.Email,
                                   func.IdIntegracaoPainel,
                                   func.RFID, 
                                    '' foto,
                                    '' contrato
                                    , horario.descricao AS jornada
                                    , convert(varchar,empresa.codigo)+' | '+ empresa.nome as empresa  
                                    , convert(varchar,departamento.codigo)+' | '+ departamento.descricao AS departamento
                                    , convert(varchar,funcao.codigo) + ' | ' + funcao.descricao AS funcao
                                    , coalesce(convert(varchar,cwu.codigo) + ' | ' + cwu.nome, '') AS supervisor    
                                    , coalesce(convert(varchar, pe.codigo) + ' | ' + pe.razaosocial, '') as PessoaSupervisor                   
                                    , convert(varchar,Alocacao.codigo) + ' | ' + Alocacao.descricao AS Alocacao       
                                    , convert(varchar,TipoVinculo.codigo) + ' | ' + TipoVinculo.descricao AS TipoVinculo
                                    , func.IdHorarioDinamico
                                    , func.CicloSequenciaIndice
                                    , convert(varchar,HDN.codigo) +' | '+HDN.descricao HorarioDinamico
                          FROM funcionario func 
                          LEFT JOIN empresa ON empresa.id = func.idempresa
                          LEFT JOIN departamento ON departamento.id = func.iddepartamento
                          LEFT JOIN horario ON horario.id = func.idhorario
                          LEFT JOIN funcao ON funcao.id = func.idFuncao
                          LEFT JOIN cw_usuario cwu ON cwu.id = func.idcw_usuario
                          LEFT JOIN pessoa pe ON pe.id = func.IdPessoaSupervisor
                          LEFT JOIN Alocacao ON alocacao.id = func.idAlocacao
                          LEFT JOIN TipoVinculo ON TipoVinculo.id = func.idTipoVinculo
                          LEFT JOIN HorarioDinamico HDN ON HDN.id = func.idhorariodinamico
                          WHERE ISNULL(func.excluido, 0) = 0 AND empresa.Id in (@empresas)
                          ORDER BY departamento, func.nome";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Modelo.Funcionario objFuncionario = new Modelo.Funcionario();
                    AuxSetInstance(dr, objFuncionario);

                    lista.Add(objFuncionario);
                }
            }
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return lista;
        }

        public int GetIdDsCodigo(string pDsCodigo)
        {
            try
            {
                SqlParameter[] parms = new SqlParameter[]
                {
                    new SqlParameter("@dscodigo", SqlDbType.VarChar)
                };
                parms[0].Value = pDsCodigo;

                string aux = "SELECT id FROM funcionario WHERE dscodigo = @dscodigo";
                aux += PermissaoUsuarioFuncionario(UsuarioLogado, aux, "funcionario.idempresa", "funcionario.id", null);
                return (int)db.ExecuteScalar(CommandType.Text, aux, parms);
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public int GetIdDsCodigoProximidade(string pDsCodigo)
        {
            try
            {
                string auxCount = "SELECT count(id) as qt FROM funcionario WHERE dscodigo like '%" + pDsCodigo + "'";
                int qtd = (int)db.ExecuteScalar(CommandType.Text, auxCount, null);
                if (qtd > 1)
                    return -1;

                string aux = "SELECT id FROM funcionario WHERE dscodigo like '%" + pDsCodigo + "'";

                return (int)db.ExecuteScalar(CommandType.Text, aux, null);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public Modelo.Funcionario LoadObjectByCodigo(int codigo)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter ("@codigo", SqlDbType.Int)
            };
            parms[0].Value = codigo;

            string aux = SqlLoadByCodigo();

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);

            Modelo.Funcionario objFunc = new Modelo.Funcionario();
            SetInstance(dr, objFunc);
            return objFunc;
        }

        public List<PxyFuncionarioExcluidoGrid> GetExcluidosList()
        {
            List<PxyFuncionarioExcluidoGrid> lista = new List<PxyFuncionarioExcluidoGrid>();

            SqlParameter[] parms = new SqlParameter[] { };

            string sql = @" SELECT func.id [Id],
		                             func.nome [Nome],
		                             func.dscodigo [Codigo],
		                             func.matricula [Matricula],
		                             CONVERT(VARCHAR, horario.codigo) + ' | ' +horario.descricao AS  [Horario],
		                             CONVERT(varchar,emp.codigo)+' | '+emp.nome as  [Empresa],
		                             CONVERT(varchar,departamento.codigo)+' | '+departamento.descricao AS  [Departamento],
                                     CONVERT(varchar,funcao.codigo) + ' | ' + funcao.descricao AS  [Funcao],
		                             func.carteira [Carteira],
		                             func.CPF [CPF],
		                             func.dataadmissao [DataAdmissao],
                                     func.datademissao [DataDemissao],
		                             func.DataInativacao [DataInativacao],
		                             func.idempresa
                                FROM funcionario func WITH (NOLOCK)
                                LEFT JOIN empresa emp ON emp.id = func.idempresa
                                LEFT JOIN departamento ON departamento.id = func.iddepartamento
                                LEFT JOIN horario ON horario.id = func.idhorario
                                LEFT JOIN funcao ON funcao.id = func.idFuncao
                               WHERE func.excluido = 1 ";
            sql += PermissaoUsuarioFuncionario(UsuarioLogado, sql, "func.idempresa", "func.id", null);
            sql += " ORDER BY func.nome ASC ";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);
            try
            {
                var mapOco = Mapper.CreateMap<IDataReader, PxyFuncionarioExcluidoGrid>();
                lista = Mapper.Map<List<PxyFuncionarioExcluidoGrid>>(dr);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (!dr.IsClosed)
                {
                    dr.Close();
                }
                dr.Dispose();
            }
            return lista;
        }

        public IList<Modelo.Proxy.pxyFuncionarioRelatorio> GetRelFuncionariosRelatorios(string filtro)
        {
            IList<Modelo.Proxy.pxyFuncionarioRelatorio> res = new List<Modelo.Proxy.pxyFuncionarioRelatorio>();
            SqlParameter[] parms = new SqlParameter[0];
            string sql = @"select f.id, 
	                               f.codigo, 
	                               f.dscodigo,
	                               f.nome,
                                   f.pis,
	                               e.id idEmpresa,
	                               convert(varchar,e.codigo) +' | '+ e.nome Empresa,
	                               d.id idDepartamento,
	                               convert(varchar,d.codigo) +' | '+ d.descricao Departamento,
	                               fu.id idFuncao,
	                               CONVERT(varchar, fu.codigo) + ' | '+ fu.descricao Funcao,
                                   f.matricula,
                                   
								       (select top(1) CONVERT(varchar, c.codigocontrato) + ' | '+ c.descricaocontrato 
								          from contratofuncionario cf
									      left join contrato c on cf.idcontrato = c.id 
									     where cf.idfuncionario = f.id
									    ) Contrato
                              from funcionario f
                             inner join empresa e on e.id = f.idempresa
                             inner join departamento d on d.id = f.iddepartamento
                             inner join funcao fu on fu.id = f.idfuncao
                             WHERE ISNULL(f.excluido, 0) = 0 " + filtro;
            sql = sql + PermissaoUsuarioFuncionario(UsuarioLogado, sql, "f.idempresa", "f.id", null);
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);


            try
            {
                var mapOco = Mapper.CreateMap<IDataReader, Modelo.Proxy.pxyFuncionarioRelatorio>();
                res = Mapper.Map<List<Modelo.Proxy.pxyFuncionarioRelatorio>>(dr);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (!dr.IsClosed)
                {
                    dr.Close();
                }
                dr.Dispose();
            }
            return res;
        }

        public IList<Modelo.Proxy.PxyFuncionarioCabecalhoRel> GetFuncionariosCabecalhoRel(IList<int> IdFuncs)
        {
            IList<Modelo.Proxy.PxyFuncionarioCabecalhoRel> res = new List<Modelo.Proxy.PxyFuncionarioCabecalhoRel>();
            SqlParameter[] parms = new SqlParameter[1]
            {
                new SqlParameter ("@idsFuncionarios", SqlDbType.VarChar)
            };
            parms[0].Value = String.Join(",", IdFuncs);

            string sql = @"SELECT F.id Idfunc
	                             , F.dscodigo
	                             , F.nome Nome
	                             , F.dataadmissao DataAdmissao
	                             , F.matricula Matricula
	                             , F.pis
                                 , F.IdEmpresa
                                 , F.IdDepartamento
                                 , F.IdFuncao
	                             , e.nome EmpresaNome
	                             , E.endereco EmpresaEndereco
	                             , E.cidade EmpresaCidade
								 , E.estado EmpresaEstado
								 , E.cep EmpresaCEP
								 , ISNULL(E.cnpj, e.cpf) EmpresaCNPJCPF
                                 , d.codigo DepartamentoCodigo
	                             , d.descricao DepartamentoNome
	                             , A.descricao AlocacaoDescricao
	                             , A.Endereco AlocacaoEndereco
                                 , t.descricao TipoVinculoDescricao
                                 , fu.codigo FuncaoCodigo
								 , fu.descricao FuncaoDescricao
	                            FROM funcionario f
	                            INNER JOIN empresa e on e.id = f.idempresa
	                            INNER JOIN departamento d on d.id = f.iddepartamento
	                            INNER JOIN funcao fu on fu.id = f.idfuncao
	                             LEFT JOIN Alocacao A on a.id = f.IdAlocacao
                                 LEFT JOIN TipoVinculo t on t.id = f.IdTipoVinculo
	                            WHERE f.id in (SELECT * FROM dbo.F_ClausulaIn(@idsFuncionarios))";
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);


            try
            {
                var mapOco = Mapper.CreateMap<IDataReader, Modelo.Proxy.PxyFuncionarioCabecalhoRel>();
                res = Mapper.Map<List<Modelo.Proxy.PxyFuncionarioCabecalhoRel>>(dr);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (!dr.IsClosed)
                {
                    dr.Close();
                }
                dr.Dispose();
            }
            return res;
        }

        /// <summary>
        ///     Método que retorna uma lista de funcionários de acordo com uma string com ids de funcionários.
        /// </summary>
        /// <param name="pIDs">Passar uma lista de ids separados por vírgula. Ex: '1,2,3,4,10,20,100'</param>
        /// <returns></returns>
        public IList<Modelo.Funcionario> GetFuncionariosPorIds(string pIDs)
        {
            IList<Modelo.Funcionario> res = new List<Modelo.Funcionario>();
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@IDs", SqlDbType.VarChar)
            };
            parms[0].Value = pIDs;
            string sql = @"select [id], [codigo], [dscodigo], [matricula], [nome], [codigofolha], [idempresa], [iddepartamento], [idfuncao], [idhorario], [tipohorario]
                                  ,[carteira], [dataadmissao], [datademissao], [salario], [funcionarioativo], [DataInativacao], [naoentrarbanco], [naoentrarcompensacao], [excluido]
                                  ,[campoobservacao], [incdata], [inchora], [incusuario], [altdata], [althora], [altusuario], [pis], [senha], [toleranciaentrada]
                                  ,[toleranciasaida], [quantidadetickets], [tipotickets], [CPF], [Mob_Senha], [idcw_usuario], [IdPessoaSupervisor], [TipoMaoObra]
                              from funcionario f
                             where f.id in (select * from dbo.F_RetornaTabelaLista(@IDs,','))";
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

            try
            {
                Mapper.CreateMap<IDataReader, Modelo.Funcionario>();
                res = Mapper.Map<List<Modelo.Funcionario>>(dr);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (!dr.IsClosed)
                {
                    dr.Close();
                }
                dr.Dispose();
            }
            return res;
        }

        public List<Modelo.Funcionario> GetAllListPorContrato(int idContrato)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter ("@idcontrato", SqlDbType.Int)
            };
            parms[0].Value = idContrato;
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, SELECTPCONT, parms);
            List<Modelo.Funcionario> lista = new List<Modelo.Funcionario>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.Funcionario>();
                lista = AutoMapper.Mapper.Map<List<Modelo.Funcionario>>(dr);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (!dr.IsClosed)
                {
                    dr.Close();
                }
                dr.Dispose();
            }
            return lista;
        }

        public List<Modelo.Funcionario> GetAllListContratos()
        {
            string sql = SELECTCONT;
            if (UsuarioLogado.UtilizaControleContratos)
            {
                sql += " where ct.id in (select cu.idcontrato from contratousuario cu where cu.idcwusuario = " + UsuarioLogado.Id + ")";
            }
            SqlParameter[] parms = new SqlParameter[] { };
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, SELECTCONT, parms);

            List<Modelo.Funcionario> lista = new List<Modelo.Funcionario>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.Funcionario>();
                lista = AutoMapper.Mapper.Map<List<Modelo.Funcionario>>(dr);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (!dr.IsClosed)
                {
                    dr.Close();
                }
                dr.Dispose();
            }
            return lista;
        }

        public int GetIdporIdIntegracao(int? IdIntegracao)
        {
            try
            {
                SqlParameter[] parms = new SqlParameter[]
                {
                    new SqlParameter("@idIntegracao", SqlDbType.Int)
                };
                parms[0].Value = IdIntegracao;

                string aux = "SELECT id FROM funcionario WHERE IdIntegracao = @idIntegracao";
                return (int)db.ExecuteScalar(CommandType.Text, aux, parms);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public List<Modelo.FechamentoPonto> FechamentoPontoFuncionario(List<int> ids)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@ids", SqlDbType.VarChar)
            };
            parms[0].Value = String.Join(",", ids);

            string sql = @"select top(3) f.id, f.codigo, f.dataFechamento, f.descricao, f.observacao
                              from FechamentoPonto f
							 inner join FechamentoPontofuncionario fpf on fpf.idFechamentoPonto = f.id
                             where fpf.idfuncionario in (select * from f_clausulain(@ids))
                            Group by f.id, f.codigo, f.dataFechamento, f.descricao, f.observacao
                            order by f.dataFechamento desc";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

            List<Modelo.FechamentoPonto> lista = new List<Modelo.FechamentoPonto>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.FechamentoPonto>();
                lista = AutoMapper.Mapper.Map<List<Modelo.FechamentoPonto>>(dr);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (!dr.IsClosed)
                {
                    dr.Close();
                }
                dr.Dispose();
            }
            return lista;
        }

        public void AtualizaHorariosFuncionariosMudanca(List<int> idsFuncionarios)
        {
            SqlParameter[] parms = new SqlParameter[1]
            {
                    new SqlParameter("@idsFuncionarios", SqlDbType.VarChar)
            };
            parms[0].Value = string.Join(",", idsFuncionarios);

            string aux = @" update  funcionario
                               set funcionario.tipohorario = ultMud.tipohorario,
	                               funcionario.idHorario = ultMud.idhorario
                              FROM    funcionario
                            OUTER APPLY
                                    (
                                    SELECT  TOP 1 *
                                    FROM    mudancahorario mh
                                    WHERE   mh.idfuncionario = funcionario.id
                                    ORDER BY mh.data DESC
                                    ) ultMud 
                             where funcionario.id in (select * from f_clausulain(@idsFuncionarios))
                               and ultMud.idfuncionario = funcionario.id ";
            SqlCommand cmd = db.ExecNonQueryCmd(CommandType.Text, aux, false, parms);
            cmd.Parameters.Clear();
        }

        public void AtualizaIdIntegracaoPainel(Modelo.Funcionario funcionarioIdIntegracaoPnl)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                    new SqlParameter("@IdIntegracaoPainel", SqlDbType.Int),
                    new SqlParameter("id", SqlDbType.Int)
            };
            parms[0].Value = funcionarioIdIntegracaoPnl.IdIntegracaoPainel;
            parms[1].Value = funcionarioIdIntegracaoPnl.Id;
            string aux = @" update funcionario
                                set IdIntegracaoPainel = @IdIntegracaoPainel
                             WHERE id = @id
                            
                          ";
            SqlCommand cmd = db.ExecNonQueryCmd(CommandType.Text, aux, false, parms);
            cmd.Parameters.Clear();
        }

        public Modelo.Funcionario GetFuncionarioPorCpfeMatricula(Int64 cpf, string matricula)
        {
                SqlParameter[] parms = new SqlParameter[]
                {
                    new SqlParameter("@cpf", SqlDbType.BigInt),
                    new SqlParameter("@matricula", SqlDbType.VarChar)
                };
                parms[0].Value = cpf;
                parms[1].Value = matricula;
                IList<Modelo.Funcionario> lista = new List<Modelo.Funcionario>();

                string sql = SqlLoadByCpfeMatricula();
                SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.Funcionario>();
                lista = AutoMapper.Mapper.Map<List<Modelo.Funcionario>>(dr);
                return lista.OrderByDescending(x => x.Funcionarioativo).FirstOrDefault();
        }

        /// <summary>
        /// Retorna uma lista com os ids dos funcionários de acordo com o parâmetro com valor maior que zero, ou seja caso id departamento > 0 traz por departamento, contrato > 0 por contrato, empresa > 0 por empresa, seguindo essa ordem
        /// </summary>
        /// <param name="idDep">Passar o id Departamento caso deseje os funcionários por Departamento</param>
        /// <param name="idCont">Passar o id Contrato caso deseje os funcionários por Contrato</param>
        /// <param name="idEmp">Passar o id Empresa caso deseje os funcionários por empresa</param>
        /// <returns>Retorna a lista de ids de funcionários dependendo dos parâmetros passados</returns>
        public List<int> GetIdsFuncsPorIdsEmpOuDepOuContra(int idDep, int idCont, int idEmp)
        {
            return GetIdsFuncsPorIdsEmpOuDepOuFuncaoOuContra(0, idDep, idCont, idEmp, false, false, false);
        }

        /// <summary>
        /// Retorna uma lista com os ids dos funcionários de acordo com o parâmetro com valor maior que zero, ou seja caso id departamento > 0 traz por departamento, funcao > 0 por funcao, contrato > 0 por contrato, empresa > 0 por empresa, seguindo essa ordem
        /// </summary>
        /// <param name="idFuncao">Passar o id Função caso deseje os funcionários por Função</param>
        /// <param name="idDep">Passar o id Departamento caso deseje os funcionários por Departamento</param>
        /// <param name="idCont">Passar o id Contrato caso deseje os funcionários por Contrato</param>
        /// <param name="idEmp">Passar o id Empresa caso deseje os funcionários por empresa</param>
        /// <returns>Retorna a lista de ids de funcionários dependendo dos parâmetros passados</returns>
        public List<int> GetIdsFuncsPorIdsEmpOuDepOuFuncaoOuContra(int idFuncao, int idDep, int idCont, int idEmp, bool verificaPermissao, bool removeInativo, bool removeExcluido)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@idEmp", SqlDbType.Int),
                new SqlParameter("@idDep", SqlDbType.Int),
                new SqlParameter("@idCont", SqlDbType.Int),
                new SqlParameter("@idFuncao", SqlDbType.Int)
            };
            parms[0].Value = idEmp;
            parms[1].Value = idDep;
            parms[2].Value = idCont;
            parms[3].Value = idFuncao;

            string sql = @" Select F.id
                              FROM Funcionario F
                              LEFT JOIN contratofuncionario CF on cf.idfuncionario = f.id
                             WHERE (@idDep > 0 and f.iddepartamento = @idDep) OR
                                   (@idFuncao > 0 and f.idfuncao = @idFuncao) OR
	                               (@idCont > 0 and CF.idcontrato = @idCont) OR
	                               (@idEmp > 0 and @idDep = 0 and @idCont = 0 and f.idempresa = @idEmp) OR
                                   (@idEmp = 0 and @idDep = 0 and @idCont = 0 and @idFuncao = 0) ";

            if (removeInativo)
            {
                sql += " and f.funcionarioativo = 1 ";
            }

            if (removeExcluido)
            {
                sql += " and f.excluido = 0 ";
            }

            if (verificaPermissao)
            {
                sql = sql + PermissaoUsuarioFuncionario(UsuarioLogado, sql, "f.idempresa", "f.id", null);
            }

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

            IList<int> lista = new List<int>();
            try
            {
                while (dr.Read())
                {
                    lista.Add(dr.GetInt32(dr.GetOrdinal("ID")));
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (!dr.IsClosed)
                {
                    dr.Close();
                }
                dr.Dispose();
            }
            return lista.ToList();
        }


        public List<Modelo.Funcionario> GetAllListPorPis(List<string> lPis)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter ("@lPis", SqlDbType.VarChar)
            };
            parms[0].Value = String.Join(",", lPis);

            string sql = @"select f.id, f.dscodigo, f.nome, f.pis, f.idempresa, f.iddepartamento, f.idfuncao, f.idhorario, f.tipohorario, f.dataadmissao,
	                               f.datademissao, f.funcionarioativo, f.DataInativacao, f.excluido, f.matricula
                              from funcionario f
                             where CONVERT(DECIMAL, replace(replace(replace(f.pis,'.',''),'-',''),'/','')) in (select CONVERT(DECIMAL, replace(replace(replace(valor,'.',''),'-',''),'/',''))  from dbo.F_ClausulaIn(@lPis))";
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);
            List<Modelo.Funcionario> lista = new List<Modelo.Funcionario>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.Funcionario>();
                lista = AutoMapper.Mapper.Map<List<Modelo.Funcionario>>(dr);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (!dr.IsClosed)
                {
                    dr.Close();
                }
                dr.Dispose();
            }
            return lista;
        }

        /// <summary>
        /// Retorna uma lista com os ids dos funcionários de acordo com o parâmetro com valor maior que zero, ou seja caso id departamento > 0 traz por departamento, contrato > 0 por contrato, empresa > 0 por empresa, seguindo essa ordem
        /// </summary>
        /// <param name="idDep">Passar o id Departamento caso deseje os funcionários por Departamento</param>
        /// <param name="idCont">Passar o id Contrato caso deseje os funcionários por Contrato</param>
        /// <param name="idEmp">Passar o id Empresa caso deseje os funcionários por empresa</param>
        /// <returns>Retorna a lista de ids de funcionários dependendo dos parâmetros passados</returns>
        public List<int> GetIdsFuncsPorIdsEmpOuDepOuContra(List<int> ListIdDep, List<int> ListIdCont, List<int> ListIdEmp)
        {
            return GetIdsFuncsPorIdsEmpOuDepOuContra(ListIdDep, ListIdCont, ListIdEmp, false, false, false);
        }
        public List<int> GetIdsFuncsPorIdsEmpOuDepOuContra(List<int> ListIdDep, List<int> ListIdCont, List<int> ListIdEmp, bool verificaPermissao, bool removeInativo, bool removeExcluido)
        {
            List<string> _where;
            string _listIdDep, _listIdCont, _listIdEmp, _sql;

            _where = new List<string>();
            _sql = @" Select F.id FROM Funcionario F LEFT JOIN contratofuncionario CF on cf.idfuncionario = f.id WHERE 1 = 1 ";

            if (ListIdEmp != null)
            {
                _listIdEmp = String.Join(",", ListIdEmp);
                _where.Add(string.Format("(f.idempresa in ({0}) ) ", _listIdEmp));
            }

            if (ListIdDep != null)
            {
                _listIdDep = String.Join(",", ListIdDep);
                _where.Add(string.Format("(f.iddepartamento in ({0}) ) ", _listIdDep));
            }

            if (ListIdCont != null)
            {
                _listIdCont = String.Join(",", ListIdCont);
                _where.Add(string.Format("(CF.idcontrato in ({0}) )", _listIdCont));
            }

            _sql += " and (" + string.Join(" or ", _where) + ")";

            if (removeInativo)
            {
                _sql += " and f.funcionarioativo = 1 ";
            }

            if (removeExcluido)
            {
                _sql += " and f.excluido = 0 ";
            }

            if (verificaPermissao)
            {
                _sql = _sql + PermissaoUsuarioFuncionario(UsuarioLogado, _sql, "f.idempresa", "f.id", null);
            }

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, _sql);

            IList<int> lista = new List<int>();
            try
            {
                while (dr.Read())
                {
                    lista.Add(dr.GetInt32(dr.GetOrdinal("ID")));
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (!dr.IsClosed)
                {
                    dr.Close();
                }
                dr.Dispose();
            }
            return lista.ToList();
        }

        public List<int> GetAllListPorCPF(List<string> lCPF)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter ("@lCPF", SqlDbType.VarChar)
            };
            parms[0].Value = String.Join(",", lCPF);

            string sql = @"select id from funcionario f
                           where CONVERT(DECIMAL, replace(replace(replace(f.CPF,'.',''),'-',''),'/','')) in
                           (select CONVERT(DECIMAL, replace(replace(replace(valor,'.',''),'-',''),'/',''))  from dbo.F_ClausulaIn(@lCPF))";
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

            IList<int> lista = new List<int>();
            try
            {
                while (dr.Read())
                {
                    lista.Add(dr.GetInt32(dr.GetOrdinal("ID")));
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (!dr.IsClosed)
                {
                    dr.Close();
                }
                dr.Dispose();
            }
            return lista.ToList();
        }

        public List<Modelo.Funcionario> GetAllFuncsListPorCPF(List<string> lCPF)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter ("@lCPF", SqlDbType.VarChar)
            };
            parms[0].Value = String.Join(",", lCPF);

            string sql = @"select f.id,
                                   f.codigo,
                                   f.dscodigo,
                                   f.matricula,
                                   f.nome,
                                   f.codigofolha,
                                   f.idempresa,
                                   f.iddepartamento,
                                   f.idfuncao,
                                   f.idhorario,
                                   f.tipohorario,
                                   f.carteira,
                                   f.dataadmissao,
                                   f.datademissao,
                                   f.salario,
                                   f.funcionarioativo,
                                   f.DataInativacao,
                                   f.naoentrarbanco,
                                   f.naoentrarcompensacao,
                                   f.excluido,
                                   f.campoobservacao,
                                   f.incdata,
                                   f.inchora,
                                   f.incusuario,
                                   f.altdata,
                                   f.althora,
                                   f.altusuario,
                                   f.pis,
                                   f.senha,
                                   f.toleranciaentrada,
                                   f.toleranciasaida,
                                   f.quantidadetickets,
                                   f.tipotickets,
                                   f.CPF,
                                   f.Mob_Senha,
                                   f.idcw_usuario,
                                   f.utilizaregistrador,
                                   f.UtilizaAppPontofopag,
                                   f.UtilizaReconhecimentoFacialApp,
                                   f.UtilizaWebAppPontofopag,
                                   f.UtilizaReconhecimentoFacialWebApp,
                                   f.idIntegracao,
                                   f.IdPessoaSupervisor,
                                   f.TipoMaoObra,
                                   f.IdAlocacao,
                                   f.IdTipoVinculo,
                                   f.Email,
                                   f.IdIntegracaoPainel
                            from funcionario f
                           where CONVERT(DECIMAL, replace(replace(replace(f.CPF,'.',''),'-',''),'/','')) in
                           (select CONVERT(DECIMAL, replace(replace(replace(valor,'.',''),'-',''),'/',''))  from dbo.F_ClausulaIn(@lCPF))";
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

            List<Modelo.Funcionario> lista = new List<Modelo.Funcionario>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.Funcionario>();
                lista = AutoMapper.Mapper.Map<List<Modelo.Funcionario>>(dr);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (!dr.IsClosed)
                {
                    dr.Close();
                }
                dr.Dispose();
            }
            return lista;
        }

        /// <summary>
        /// Retorna os funcionários com o pis duplicado
        /// </summary>
        /// <param name="lPis">Quando passado pis como parâmetro retornará apenas os registros que possuem o mesmo dentro da lista, caso o parametros estaja vazio retorna todos os funcs com mais de um pis
        /// Select não desconsidera inativos e excluídos
        /// </param>
        /// <returns> Retorna os funcionários com o pis duplicado sem desconsiderar excluido ou inativo</returns>
        public List<Modelo.Funcionario> GetAllPisDuplicados(List<string> lPis)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter ("@lPis", SqlDbType.VarChar)
            };
            parms[0].Value = "";
            if (lPis.Count() > 0)
            {
                parms[0].Value = String.Join(",", lPis);
            }

            string sql = @" SELECT *
                              FROM (
	                            SELECT  f.id ,
			                            f.dscodigo ,
			                            f.nome ,
			                            f.pis ,
			                            f.idempresa ,
			                            f.iddepartamento ,
			                            f.idfuncao ,
			                            f.idhorario ,
			                            f.tipohorario ,
			                            f.dataadmissao ,
			                            f.datademissao ,
			                            f.funcionarioativo ,
                                        f.DataInativacao,
			                            f.excluido,
			                            COUNT(*) OVER(PARTITION BY f.pis ORDER BY pis) qtd
	                            FROM    funcionario f
	                            WHERE   (CONVERT(DECIMAL, REPLACE(REPLACE(REPLACE(f.pis, '.', ''), '-', ''),
									                             '/', '')) IN (
			                            SELECT  CONVERT(DECIMAL, REPLACE(REPLACE(REPLACE(valor, '.', ''), '-',
													                             ''), '/', ''))
			                            FROM    dbo.F_ClausulaIn(@lPis) ) OR
                                        @lPis = '')
								  AND f.excluido = 0
								  AND f.funcionarioativo = 1
		                            ) p WHERE qtd > 1";
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);
            List<Modelo.Funcionario> lista = new List<Modelo.Funcionario>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.Funcionario>();
                lista = AutoMapper.Mapper.Map<List<Modelo.Funcionario>>(dr);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (!dr.IsClosed)
                {
                    dr.Close();
                }
                dr.Dispose();
            }
            return lista;
        }

        public List<Modelo.Funcionario> GetPorHorarioVigencia(int idHorario)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter ("@idHorario", SqlDbType.Int)
            };
            parms[0].Value = idHorario;

            string sql = @" SELECT  func.id,
                                       func.codigo,
                                       func.dscodigo,
                                       func.matricula,
                                       func.nome,
                                       func.codigofolha,
                                       func.idempresa,
                                       func.iddepartamento,
                                       func.idfuncao,
                                       func.idhorario,
                                       func.tipohorario,
                                       func.carteira,
                                       func.dataadmissao,
                                       func.datademissao,
                                       func.salario,
                                       func.funcionarioativo,
                                       func.DataInativacao,
                                       func.naoentrarbanco,
                                       func.naoentrarcompensacao,
                                       func.excluido,
                                       func.campoobservacao,
                                       func.incdata,
                                       func.inchora,
                                       func.incusuario,
                                       func.altdata,
                                       func.althora,
                                       func.altusuario,
                                       func.pis,
                                       func.senha,
                                       func.toleranciaentrada,
                                       func.toleranciasaida,
                                       func.quantidadetickets,
                                       func.tipotickets,
                                       func.CPF,
                                       func.Mob_Senha,
                                       func.idcw_usuario,
                                       func.utilizaregistrador,
                                       func.UtilizaAppPontofopag,
                                       func.UtilizaReconhecimentoFacialApp,
                                       func.UtilizaWebAppPontofopag,
                                       func.UtilizaReconhecimentoFacialWebApp,
                                       func.idIntegracao,
                                       func.IdPessoaSupervisor,
                                       func.TipoMaoObra,
                                       func.IdAlocacao,
                                       func.IdTipoVinculo,
                                       func.Email,
                                       func.IdIntegracaoPainel
                            FROM    dbo.funcionario func
                            WHERE   ISNULL(func.excluido, 0) = 0
                                    AND ISNULL(func.funcionarioativo, 0) = 1
                                    AND func.idhorario IN ( SELECT  h.id
                                                            FROM    dbo.horario h
                                                            WHERE   h.IdHorarioOrigem = @idHorario
                                                                    OR h.id = @idHorario ); ";
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);
            List<Modelo.Funcionario> lista = new List<Modelo.Funcionario>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.Funcionario>();
                lista = AutoMapper.Mapper.Map<List<Modelo.Funcionario>>(dr);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (!dr.IsClosed)
                {
                    dr.Close();
                }
                dr.Dispose();
            }
            return lista;
        }

        public Modelo.Funcionario LoadObjectByPis(string pis)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter ("@pis", SqlDbType.VarChar)
            };
            parms[0].Value = pis;

            string aux = SqlLoadByPis();

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);

            Modelo.Funcionario objFunc = new Modelo.Funcionario();
            SetInstance(dr, objFunc);
            return objFunc;
        }

        public List<Modelo.Proxy.PxyFuncionarioDiaUtil> GetDiaUtilFuncionario(List<int> idsFuncs, DateTime dataIni, DateTime dataFin)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@idFuncionario", SqlDbType.VarChar),
                new SqlParameter("@datai", SqlDbType.DateTime),
                new SqlParameter("@dataf", SqlDbType.DateTime)
            };
            if (idsFuncs.Count > 0)
                parms[0].Value = String.Join(",", idsFuncs);
            else parms[0].Value = "";
            parms[1].Value = dataIni;
            parms[2].Value = dataFin;

            string sql = @"SELECT *, 
	                               CASE WHEN PossuiJornada = 1 AND t.PossuiFeriado = 0 AND t.PossuiFolga = 0 AND t.PossuiAfastamento = 0 THEN 1 ELSE 0 END DeveTrabalhar
                              FROM (
	                            SELECT func.id IdFuncionario, 
			                            dt.data Data, 
			                            ISNULL(ja.idjornada, hd.idjornada) IdJornada, 
			                            (CASE WHEN hd.entrada_1 <> '--:--' THEN hd.entrada_2 ELSE '' END +
                                         CASE WHEN hd.saida_1 <> '--:--' THEN ' - ' + hd.saida_2 ELSE '' END +
                                         CASE WHEN hd.entrada_2 <> '--:--' THEN ' - ' + hd.entrada_2 ELSE '' END +
                                         CASE WHEN hd.saida_2 <> '--:--' THEN ' - ' + hd.saida_2 ELSE '' END +
                                         CASE WHEN hd.entrada_3 <> '--:--' THEN ' - ' + hd.entrada_3 ELSE '' END +
                                         CASE WHEN hd.saida_3 <> '--:--' THEN ' - ' + hd.saida_3 ELSE '' END +
                                         CASE WHEN hd.entrada_4 <> '--:--' THEN ' - ' + hd.entrada_4 ELSE '' END +
                                         CASE WHEN hd.saida_4 <> '--:--' THEN ' - ' + hd.saida_4 ELSE '' END 
                                        ) AS horarios, 
			                            f.id IdFeriado, 
			                            f.descricao DescFeriado,
			                            marc.folga Folga,
			                            afast.id IdAfastamento,
			                            o.descricao DescAfastamento,
			                            SIGN(ISNULL(ISNULL(ja.idjornada, hd.idjornada),0)) PossuiJornada, 
			                            SIGN(ISNULL(f.id,0)) PossuiFeriado,
			                            ISNULL(folga,0) PossuiFolga, 
			                            SIGN(ISNULL(afast.id,0)) PossuiAfastamento
	                              FROM dbo.FN_DatasPeriodo(@datai, @dataf) dt
	                              LEFT JOIN  dbo.funcionario func ON func.id IN (SELECT * FROM dbo.F_ClausulaIn(@idFuncionario)) OR @idFuncionario = ''
	                              LEFT JOIN dbo.horario AS h ON func.idhorario = h.id 
	                              LEFT JOIN dbo.horariodetalhe AS hd ON h.id = hd.idhorario AND (h.tipohorario = 1) AND (hd.dia = CASE WHEN DATEPART(DW, dt.data) -1 = 0 THEN 7 ELSE DATEPART(DW, dt.data) -1 END ) OR
																	                            (h.tipohorario = 2) AND (hd.data = dt.data)
	                              LEFT JOIN dbo.jornadaalternativa AS ja ON (ja.tipo = 0 AND ja.identificacao = func.idempresa OR
												                             ja.tipo = 1 AND ja.identificacao = func.iddepartamento OR
												                             ja.tipo = 2 AND ja.identificacao = func.id) AND dt.data BETWEEN ja.datainicial AND ja.datafinal
	                              LEFT JOIN dbo.feriado f ON dt.data = f.data AND (f.tipoferiado = 0 or
														                            (f.tipoferiado = 1 and f.idempresa = func.idempresa) or
														                            (f.tipoferiado = 2 and f.iddepartamento = func.iddepartamento) or
														                            (f.tipoferiado = 3 and EXISTS (SELECT * FROM  dbo.FeriadoFuncionario fu WHERE f.id = fu.idFeriado AND fu.idFuncionario = func.id)))
	                              LEFT JOIN dbo.marcacao marc ON marc.idfuncionario = func.id AND marc.data = dt.data
	                              LEFT JOIN dbo.afastamento afast ON dt.data BETWEEN afast.datai AND isnull(afast.dataf, '9999-12-31') AND afast.abonado = 1 AND afast.parcial = 0 AND (afast.idfuncionario = func.id OR 
																							                            afast.idempresa = func.idempresa OR
                                                                                                                        afast.iddepartamento = func.iddepartamento OR
																							                            afast.idcontrato IN (SELECT cf.idcontrato FROM dbo.contratofuncionario cf WHERE cf.idfuncionario = func.id and cf.excluido = 0))
								  LEFT JOIN dbo.ocorrencia o ON afast.idocorrencia = o.id
		                            ) t";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, sql, parms);

            List<Modelo.Proxy.PxyFuncionarioDiaUtil> lista = new List<Modelo.Proxy.PxyFuncionarioDiaUtil>();
            try
            {
                AutoMapper.Mapper.CreateMap<IDataReader, Modelo.Proxy.PxyFuncionarioDiaUtil>();
                lista = AutoMapper.Mapper.Map<List<Modelo.Proxy.PxyFuncionarioDiaUtil>>(dr);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (!dr.IsClosed)
                {
                    dr.Close();
                }
                dr.Dispose();
            }
            return lista;
        }

        public DataTable CarregarTodosParaAPI()
        {
            return CarregarTodosParaAPI(1, 0);
        }

        /// <summary>
        /// Método para retornar todos os funcionários com opção de filtro por excluídos, Inativos ou todos
        /// </summary>
        /// <param name="Ativo">0 = Inativo, 1 = Ativo e 2 = Todos</param>
        /// <param name="Excluido">0 = Não Excluído, 1 = Excluído e 2 = Todos</param>
        /// <returns>Retornar DataTable com Funcionários</returns>
        public DataTable CarregarTodosParaAPI(Int16 ativo, Int16 excluido)
        {
            SqlParameter[] parms = new SqlParameter[]
            {
                new SqlParameter("@Ativo", SqlDbType.SmallInt),
                new SqlParameter("@Excluido", SqlDbType.SmallInt)
            };
            parms[0].Value = ativo;
            parms[1].Value = excluido;
            string comando = @" SELECT      func.id
                                        , func.nome
                                        , func.matricula
                                        , func.cpf
									    , func.funcionarioativo
									    , func.excluido
									    , e.cnpj
									    , e.nome NomeEmpresa
                                 FROM funcionario func
							    INNER JOIN dbo.empresa e ON func.idempresa = e.id
							    WHERE (@ativo = 2 OR func.funcionarioativo = @ativo)
							      AND (@Excluido = 2 OR func.excluido = @Excluido)
                                 ORDER BY func.nome";

            DataTable dt = new DataTable();
            using (IDataReader dr = db.ExecuteReader(CommandType.Text, comando, parms))
            {
                dt.Load(dr);
            }
            return dt;
        }

        public DataTable CarregarHorarioMarcacao(int idMarcacao, int diaSemana)
        {
            SqlParameter[] parms = new SqlParameter[] {
                new SqlParameter { ParameterName = "@id", Value = idMarcacao },
                new SqlParameter { ParameterName = "@dia", Value = diaSemana },
            };
            string comando = @"
select 
	jo.entrada_1,
	jo.entrada_2,
	jo.entrada_3,
	jo.entrada_4,
	jo.saida_1,
	jo.saida_2,
	jo.saida_3,
	jo.saida_4
from
	marcacao mar
	left join funcionario fun on fun.id = mar.idfuncionario
	left join horario hor on hor.id = mar.idhorario
	left join horariodetalhe det on det.idhorario = hor.id
	left join jornadaalternativa joa on ( 
		( 0=1
			or (joa.tipo = 0 and joa.identificacao = fun.idempresa)
			or (joa.tipo = 1 and joa.identificacao = fun.iddepartamento)
			or (joa.tipo = 2 and joa.identificacao = fun.id)
		) 
		and	(
			mar.data between joa.datainicial and joa.datafinal
		)
	)
	inner join jornada jo on jo.id = coalesce(joa.idjornada, det.idjornada)
where 1=1
	and mar.id = @id
	and det.dia = @dia
";

            DataTable dt = new DataTable();
            using (IDataReader dr = db.ExecuteReader(CommandType.Text, comando, parms))
            {
                dt.Load(dr);
            }
            return dt;
        }

        public List<int> GetIDsByDsCodigos(List<string> lDsCodigos)
        {
            DataTable dt = new DataTable();
            string aux = @" SELECT id FROM dbo.funcionario WHERE dscodigo IN ( '"+String.Join("','",lDsCodigos)+"') ";

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, null);
            dt.Load(dr);
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();
            List<int> ids = dt.AsEnumerable().Select(x => Convert.ToInt32(x[0])).ToList();
            return ids;
        }


        public DataTable GetPeriodoFechamentoPonto(List<int> idsFuncs)
        {
            SqlParameter[] parms = new SqlParameter[1]
            {
                    new SqlParameter("@idsFuncs", SqlDbType.Structured)
            };
            parms[0].Value = CreateDataTableIdentificadores(idsFuncs.Select(s => (long)s));
            parms[0].TypeName = "Identificadores";

            #region Select Otimizado
            string aux = @"
                SELECT idfuncionario, 
		                IIF(DiaFechamentoInicial = 1 AND DiaFechamentoFinal = 30, 0,  DiaFechamentoInicial) DiaFechamentoInicial, 
		                IIF(DiaFechamentoInicial = 1 AND DiaFechamentoFinal = 30, 0,  DiaFechamentoFinal) DiaFechamentoFinal
                  FROM (
                SELECT PF.*,
	                   ROW_NUMBER() OVER(PARTITION BY PF.idfuncionario ORDER BY PF.idfuncionario, PF.prioridade) Ordem
                  FROM  (
		                SELECT t.idfuncionario, t.DiaFechamentoInicial, t.DiaFechamentoFinal, 1 prioridade
		                  FROM (
			                SELECT cf.idfuncionario, c.DiaFechamentoInicial, c.DiaFechamentoFinal,
					                ROW_NUMBER() OVER(PARTITION BY cf.idfuncionario ORDER BY c.DiaFechamentoInicial desc, c.incdata desc) ordem
			                  FROM dbo.contrato c
			                 INNER JOIN dbo.contratofuncionario cf ON c.id = cf.idcontrato
			                 INNER JOIN @idsFuncs i ON i.Identificador = cf.idfuncionario
			                   AND c.DiaFechamentoInicial > 0 AND c.DiaFechamentoFinal > 0
			                   ) t
		                   WHERE t.ordem = 1
		                union
		                SELECT f.id, e.DiaFechamentoInicial, e.DiaFechamentoFinal, 2 prioridade
		                  FROM dbo.funcionario f
		                 INNER JOIN @idsFuncs i ON i.Identificador = f.id
		                 INNER JOIN dbo.empresa e ON e.id = f.idempresa
		                 WHERE e.DiaFechamentoInicial > 0 AND e.DiaFechamentoFinal > 0
		                UNION
		                SELECT f.id, p.DiaFechamentoInicial, p.DiaFechamentoFinal, 3 prioridade
		                  FROM dbo.funcionario f
		                  INNER JOIN @idsFuncs i ON i.Identificador = f.id
		                  CROSS JOIN (  SELECT TOP 1 * FROM parametros ORDER BY parametros.codigo) p
	                ) PF
	                ) X WHERE x.ordem = 1
                ";
            #endregion

            DataTable dt = new DataTable();
            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            dt.Load(dr);
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();

            return dt;
        }

        public List<PxyUltimoFechamentoPonto> GetUltimoFechamentoPontoFuncionarios(List<int> idsFuncs)
        {
            SqlParameter[] parms = new SqlParameter[1]
            {
                    new SqlParameter("@idsFuncs", SqlDbType.Structured)
            };
            parms[0].Value = CreateDataTableIdentificadores(idsFuncs.Select(s => (long)s));
            parms[0].TypeName = "Identificadores";

            #region Select
            string aux = @"
                select *,
                        case WHEN UltimoFechamentoPonto IS NULL and UltimoFechamentoBanco IS NOT NULL THEN
                                 UltimoFechamentoBanco
                             WHEN UltimoFechamentoPonto IS NOT NULL AND UltimoFechamentoBanco IS NULL THEN
		                        UltimoFechamentoPonto
                             WHEN UltimoFechamentoPonto > UltimoFechamentoBanco THEN
		                        UltimoFechamentoPonto
                             ELSE UltimoFechamentoBanco end UltimoFechamento
                          from (
	                        select f.dscodigo Codigo,
		                           f.CPF,
		                           f.matricula Matricula,
		                           f.pis Pis,
		                           f.nome Nome,
		                           (select max(fp.dataFechamento) from FechamentoPonto fp inner join FechamentoPontoFuncionario fpf on fp.id = fpf.idFechamentoPonto and fpf.idfuncionario = f.id) UltimoFechamentoPonto,
		                           (select max(fbh.data) from fechamentobh fbh inner join fechamentobhd fbhd on fbh.id = fbhd.idfechamentobh and fbhd.identificacao = f.id) UltimoFechamentoBanco
	                        from funcionario f
	                        inner JOIN @idsFuncs t on t.Identificador = f.id
                        ) t
                ";
            #endregion

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            List<PxyUltimoFechamentoPonto> lista = new List<PxyUltimoFechamentoPonto>();
            try
            {
                Mapper.CreateMap<IDataReader, PxyUltimoFechamentoPonto>();
                lista = Mapper.Map<List<PxyUltimoFechamentoPonto>>(dr);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                if (!dr.IsClosed)
                {
                    dr.Close();
                }
                dr.Dispose();
            }
            return lista;
        }

        /// <summary>
        /// Retorna a lista de Ids funcionários que estão/estavam Contratados no paríodo informado
        /// </summary>
        /// <param name="tipo">Tipo do filtro</param>
        /// <param name="idsReg">Ids do Filtro</param>
        /// <param name="DtIni">Data Inicio</param>
        /// <param name="DtFin">Data Fim</param>
        /// <returns>Ids funcionários</returns>
        public List<int> IdsFuncPeriodoContratado(TipoFiltroFuncionario tipo, List<int> idsReg, DateTime dtIni, DateTime dtFin)
        {
            SqlParameter[] parms = new SqlParameter[2]
            {
                    new SqlParameter("@datainicial", SqlDbType.DateTime),
                    new SqlParameter("@datafinal", SqlDbType.DateTime)
            };

            parms[0].Value = dtIni;
            parms[1].Value = dtFin;
            DataTable dt = new DataTable();
            string aux = @" SELECT id 
                                    FROM funcionario WITH (NOLOCK)
                                    WHERE 1 = 1  
                                    AND ISNULL(funcionario.excluido,0) = 0 
                                    AND (funcionario.dataadmissao BETWEEN @datainicial and @datafinal OR
		                                isnull(funcionario.datademissao,'2999-12-01') BETWEEN @datainicial and @datafinal OR
		                                @datainicial BETWEEN funcionario.dataadmissao and isnull(funcionario.datademissao,'2999-12-01') OR
		                                @datafinal BETWEEN funcionario.dataadmissao and isnull(funcionario.datademissao,'2999-12-01')) ";


            switch (tipo)
            {
                case TipoFiltroFuncionario.Empresa:
                    aux += string.Format(" and funcionario.idempresa = {0}", string.Join(",", idsReg));
                    break;
                case TipoFiltroFuncionario.Departamento:
                    aux += string.Format(" and funcionario.iddepartamento = {0}", string.Join(",", idsReg));
                    break;
                case TipoFiltroFuncionario.Funcionario:
                    aux += string.Format(" and funcionario.id = {0}", string.Join(",", idsReg));
                    break;
                case TipoFiltroFuncionario.Funcao:
                    aux += string.Format(" and funcionario.idfuncao = {0}", string.Join(",", idsReg));
                    break;
                case TipoFiltroFuncionario.Horario:
                    aux += string.Format(" and funcionario.idhorario = {0}", string.Join(",", idsReg));
                    break;
                default:
                    break;
            }

            SqlDataReader dr = db.ExecuteReader(CommandType.Text, aux, parms);
            dt.Load(dr);
            if (!dr.IsClosed)
                dr.Close();
            dr.Dispose();
            List<int> ids = dt.AsEnumerable().Select(x => Convert.ToInt32(x[0])).ToList();
            return ids;
        }

    }
}
