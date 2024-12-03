document.addEventListener('DOMContentLoaded', function () {
    // Configuração de máscaras
    const cpfInput = document.getElementById('CPF');
    if (cpfInput) {
        IMask(cpfInput, { mask: '000.000.000-00' });
    }

    const rgInput = document.getElementById('RG');
    if (rgInput) {
        IMask(rgInput, { mask: '00.000.000-0' });
    }

    const cepInput = document.getElementById('Endereco_CEP');
    if (cepInput) {
        IMask(cepInput, { mask: '00000-000' });

        
        cepInput.addEventListener('blur', function () {
            const cep = this.value.replace(/\D/g, ''); 
            if (cep.length === 8) {
                fetch(`https://viacep.com.br/ws/${cep}/json/`)
                    .then(response => {
                        if (!response.ok) throw new Error('Erro na resposta da API');
                        return response.json();
                    })
                    .then(data => {
                        if (!data.erro) {
                            document.querySelector("input[name='Endereco.Logradouro']").value = data.logradouro || '';
                            document.querySelector("input[name='Endereco.Bairro']").value = data.bairro || '';
                            document.querySelector("input[name='Endereco.Localidade']").value = data.localidade || ''; // Alterado de Cidade para Localidade
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
                alert("CEP inválido! Digite um CEP com 8 dígitos.");
            }
        });
    }
});
