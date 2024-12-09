
        $(document).ready(function () {
            var cpfMask = IMask(document.getElementById('CPF'), {
                mask: '000.000.000-00'
            });

            var rgMask = IMask(document.getElementById('RG'), {
                mask: '00.000.000-0'
            });

            $('#CEP').on('input', function () {
                var value = $(this).val().replace(/\D/g, '');
                if (value.length > 5) {
                    value = value.slice(0, 5) + '-' + value.slice(5);
                }
                $(this).val(value);
            });

            $('#CEP').on('blur', function () {
                var cep = $(this).val().replace(/\D/g, '');
                if (cep.length === 8) {
                    $.getJSON(`https://viacep.com.br/ws/${cep}/json/`, function (data) {
                        if (!data.erro) {
                            $('#Logradouro').val(data.logradouro);
                            $('#Bairro').val(data.bairro);
                            $('#Localidade').val(data.localidade);
                            $('#UF').val(data.uf);
                        } else {
                            $('#Logradouro, #Bairro, #Localidade, #UF').val('');
                            alert('CEP não encontrado.');
                        }
                    }).fail(function () {
                        alert('Erro ao buscar o CEP.');
                    });
                }
            });
        });
    