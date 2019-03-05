using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace CasaDoCodigo.OrdemDeCompra.Models
{
    public class Pedido : BaseModel
    {
        public Pedido()
        {

        }

        public Pedido(List<ItemPedido> itens, string clienteId, string clienteNome, string clienteEmail, string clienteTelefone, string clienteEndereco, string clienteComplemento, string clienteBairro, string clienteMunicipio, string clienteUF, string clienteCEP)
        {
            Itens = itens;
            ClienteId = clienteId;
            ClienteNome = clienteNome;
            ClienteEmail = clienteEmail;
            ClienteTelefone = clienteTelefone;
            ClienteEndereco = clienteEndereco;
            ClienteComplemento = clienteComplemento;
            ClienteBairro = clienteBairro;
            ClienteMunicipio = clienteMunicipio;
            ClienteUF = clienteUF;
            ClienteCEP = clienteCEP;
        }

        [DataMember]
        public List<ItemPedido> Itens { get; private set; } = new List<ItemPedido>();
        [MinLength(5, ErrorMessage = "Nome deve ter no m�nimo 5 caracteres")]
        [MaxLength(50, ErrorMessage = "Nome deve ter no m�ximo 50 caracteres")]
        [Required(ErrorMessage = "ClienteId � obrigat�rio")]
        [DataMember]
        public string ClienteId { get; set; } = "";
        [Required(ErrorMessage = "Nome � obrigat�rio")]
        [DataMember]
        public string ClienteNome { get; set; } = "";
        [Required(ErrorMessage = "Email � obrigat�rio")]
        [DataMember]
        public string ClienteEmail { get; set; } = "";
        [Required(ErrorMessage = "Telefone � obrigat�rio")]
        [DataMember]
        public string ClienteTelefone { get; set; } = "";
        [Required(ErrorMessage = "Endereco � obrigat�rio")]
        [DataMember]
        public string ClienteEndereco { get; set; } = "";
        [Required(ErrorMessage = "Complemento � obrigat�rio")]
        [DataMember]
        public string ClienteComplemento { get; set; } = "";
        [Required(ErrorMessage = "Bairro � obrigat�rio")]
        [DataMember]
        public string ClienteBairro { get; set; } = "";
        [Required(ErrorMessage = "Municipio � obrigat�rio")]
        [DataMember]
        public string ClienteMunicipio { get; set; } = "";
        [Required(ErrorMessage = "UF � obrigat�rio")]
        [DataMember]
        public string ClienteUF { get; set; } = "";
        [Required(ErrorMessage = "CEP � obrigat�rio")]
        [DataMember]
        public string ClienteCEP { get; set; } = "";
        [DataMember]
        public DateTime DateCreated { get; set; } = DateTime.Now;

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
