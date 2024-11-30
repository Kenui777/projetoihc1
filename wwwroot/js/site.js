document.addEventListener('DOMContentLoaded', function () {
    if (document.getElementById('CPF')) {
        IMask(document.getElementById('CPF'), { mask: '000.000.000-00' });
    }

    if (document.getElementById('RG')) {
        IMask(document.getElementById('RG'), { mask: '00.000.000-0' });
    }

    if (document.getElementById('CEP')) {
        IMask(document.getElementById('CEP'), { mask: '00000-0000' });
    }

    // Busca de endereço por CEP
    document.getElementById('CEP').addEventListener('blur', function () {
        const cep = this.value.replace(/\D/g, ''); // Remove caracteres não numéricos
        if (cep && cep.length === 9) {
            fetch(`/Clientes/PreencherEndereco?cep=${cep}`)
                .then(response => response.json())
                .then(data => {
                    if (data) {
                        // Preenche os campos com os dados do endereço
                        document.querySelector("input[name='Endereco.Logradouro']").value = data.logradouro || '';
                        document.querySelector("input[name='Endereco.Bairro']").value = data.bairro || '';
                        document.querySelector("input[name='Endereco.Cidade']").value = data.localidade || '';
                        document.querySelector("input[name='Endereco.UF']").value = data.uf || '';
                        document.querySelector("input[name='Endereco.Complemento']").value = data.complemento || '';
                    } else {
                        alert("Endereço não encontrado!");
                    }
                })
                .catch(() => {
                    alert("Erro ao buscar o endereço.");
                });
        } else {
            alert("CEP inválido!");
        }
    });
});
