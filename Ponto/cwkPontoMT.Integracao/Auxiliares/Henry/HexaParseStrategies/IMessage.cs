using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace cwkPontoMT.Integracao.Auxiliares.Henry.HexaParseStrategies
{
    public abstract class IMessage
    {
        public virtual int Indice { get; set; }
        public virtual string Comando { get; set; }
        public virtual int Info { get; set; }

        protected Dictionary<int, string> PopularErros()
        {
            Dictionary<int, string> erros = new Dictionary<int, string>();

            erros.Add(1, "Não há dados");
            erros.Add(2, "Reservado");
            erros.Add(3, "Reservado");
            erros.Add(4, "Reservado");
            erros.Add(5, "Reservado");
            erros.Add(6, "Reservado");
            erros.Add(7, "Reservado");
            erros.Add(8, "Reservado");
            erros.Add(9, "Reservado");
            erros.Add(10, "Comando desconhecido");
            erros.Add(11, "Tamanho do pacote é inválido");
            erros.Add(12, "Parâmetros informados são inválidos");
            erros.Add(13, "Erro de checksum");
            erros.Add(14, "Tamanho dos parâmetros são inválidos");
            erros.Add(15, "Número da mensagem é inválido");
            erros.Add(16, "Start Byte é inválido");
            erros.Add(17, "Erro para receber pacote");
            erros.Add(18, "Reservado");
            erros.Add(19, "Reservado");
            erros.Add(20, "Não há empregador cadastrado");
            erros.Add(21, "Não há usuários cadastrados");
            erros.Add(22, "Usuário não cadastrado");
            erros.Add(23, "Usuário já cadastrado");
            erros.Add(24, "Limite de cadastro de usuários atingido");
            erros.Add(25, "Equipamento não possui biometria");
            erros.Add(26, "Index biométrico não encontrado");
            erros.Add(27, "Limite de cadastro de digitais atingido");
            erros.Add(28, "Equipamento não possui eventos");
            erros.Add(29, "Erro na manipulação de biometrias");
            erros.Add(30, "Documento do empregador é inválido");
            erros.Add(31, "Tipo do documento do empregador é inválido");
            erros.Add(32, "Ip é inválido");
            erros.Add(33, "Tipo de operação do usuário é inválida");
            erros.Add(34, "PIS do empregado é inválido");
            erros.Add(35, "Cei do empregador é inválido");
            erros.Add(36, "Referencia do empregado é inválido");
            erros.Add(37, "Nome do empregado é inválido");
            erros.Add(38, "Reservado");
            erros.Add(39, "Reservado");
            erros.Add(40, "MRP está cheia");
            erros.Add(41, "Erro ao gravar dados na MRP");
            erros.Add(42, "Erro ao ler dados da MRP");
            erros.Add(43, "Erro ao gravar dados na MT");
            erros.Add(44, "Erro ao ler dados da MT");
            erros.Add(45, "Reservado");
            erros.Add(46, "Reservado");
            erros.Add(47, "Reservado");
            erros.Add(48, "Reservado");
            erros.Add(49, "Reservado");
            erros.Add(50, "Erro desconhecido");
            erros.Add(51, "Reservado");
            erros.Add(52, "Reservado");
            erros.Add(53, "Reservado");
            erros.Add(54, "Reservado");
            erros.Add(55, "Reservado");
            erros.Add(56, "Reservado");
            erros.Add(57, "Reservado");
            erros.Add(58, "Reservado");
            erros.Add(59, "Reservado");
            erros.Add(60, "Reservado");
            erros.Add(61, "Matrícula já existe");
            erros.Add(62, "PIS já existe");
            erros.Add(63, "Opção inválida");
            erros.Add(64, "Matrícula não existe");
            erros.Add(65, "PIS não existe");
            erros.Add(66, "Reservado");
            erros.Add(67, "Reservado");
            erros.Add(68, "Reservado");
            erros.Add(69, "Reservado");
            erros.Add(70, "Reservado");
            erros.Add(71, "Reservado");
            erros.Add(72, "Reservado");
            erros.Add(73, "Reservado");
            erros.Add(74, "Reservado");
            erros.Add(75, "Reservado");
            erros.Add(76, "Reservado");
            erros.Add(77, "Reservado");
            erros.Add(78, "Reservado");
            erros.Add(79, "Reservado");
            erros.Add(80, "Reservado");
            erros.Add(81, "Reservado");
            erros.Add(82, "Reservado");
            erros.Add(83, "Reservado");
            erros.Add(84, "Reservado");
            erros.Add(85, "Reservado");
            erros.Add(86, "Reservado");
            erros.Add(87, "Reservado");
            erros.Add(88, "Reservado");
            erros.Add(89, "Reservado");
            erros.Add(90, "Reservado");
            erros.Add(91, "Reservado");
            erros.Add(92, "Reservado");
            erros.Add(93, "Reservado");
            erros.Add(94, "Reservado");
            erros.Add(95, "Reservado");
            erros.Add(96, "Reservado");
            erros.Add(97, "Reservado");
            erros.Add(98, "Reservado");
            erros.Add(99, "Reservado");
            erros.Add(100, "Comando desconhecido");
            erros.Add(101, "Erro de checksum");
            erros.Add(102, "Start Byte é inválido");
            erros.Add(103, "Parâmetros informados são inválidos");
            erros.Add(104, "Tamanho dos parâmetros é inválido");
            erros.Add(105, "Comando informado é inválido");
            erros.Add(106, "Data hora do comando é inválida");
            erros.Add(107, "Evento é inválido");
            erros.Add(108, "Tamanho do evento é inválido");
            erros.Add(109, "Código de inserção é inválido");
            erros.Add(110, "Parâmetros excedem o limite");
            erros.Add(111, "NSR inválido");
            erros.Add(112, "Erro ao inicializar o SD Card");
            erros.Add(113, "Erro ao detectar o SD Card");
            erros.Add(114, "Erro ao montar SD Card");
            erros.Add(115, "Não há espaço na MRP");
            erros.Add(116, "Erro ao obter informações sobre o SD Card");
            erros.Add(117, "Erro ao abrir arquivo MRP");
            erros.Add(118, "Erro ao gravar dados na MRP");
            erros.Add(119, "Erro ao ler dados da MRP");
            erros.Add(120, "Erro ao setar posição");
            erros.Add(121, "Erro ao sincronizar");

            return erros;
        }

    }
}
