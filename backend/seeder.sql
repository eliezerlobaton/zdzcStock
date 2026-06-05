-- =============================================================
--  ZdzcStock — Seeder de Dados para Desenvolvimento
--  Banco: stock
--  10 categorias x 10 produtos = 100 produtos no total
--
--  Execução via Docker (sem instalar nada):
--  docker cp seeder.sql zstock_db:/seeder.sql
--  docker exec -it zstock_db /opt/mssql-tools18/bin/sqlcmd \
--    -S localhost -U sa -P Admin123 -No -i /seeder.sql
-- =============================================================

USE stock;
GO

SET NOCOUNT ON;

BEGIN TRANSACTION;

BEGIN TRY

    -- -------------------------------------------------------
    -- Guarda a data atual para consistência em todos os
    -- registros inseridos nesta execução.
    -- -------------------------------------------------------
    DECLARE @Now DATETIME2 = GETUTCDATE();

    -- -------------------------------------------------------
    -- Proteção idempotente:
    -- Não executa se já existirem categorias na base.
    -- -------------------------------------------------------
    IF EXISTS (SELECT 1 FROM Categories)
    BEGIN
        PRINT 'Seeder ignorado: o banco já contém dados.';
        ROLLBACK TRANSACTION;
        RETURN;
    END

    -- ===========================================================
    --  CATEGORIAS
    -- ===========================================================
    INSERT INTO Categories (Name, Description, CreatedAt, UpdatedAt)
    VALUES
        ('Eletrônicos',        'Dispositivos e equipamentos eletrônicos para uso pessoal e profissional.', @Now, @Now),
        ('Vestuário',          'Roupas, calçados e acessórios masculinos e femininos.',                   @Now, @Now),
        ('Alimentos',          'Produtos alimentícios não perecíveis e industrializados.',                @Now, @Now),
        ('Bebidas',            'Bebidas alcoólicas, não alcoólicas, sucos e energéticos.',               @Now, @Now),
        ('Higiene Pessoal',    'Produtos de higiene e cuidados pessoais diários.',                       @Now, @Now),
        ('Limpeza Doméstica',  'Produtos de limpeza e conservação para o lar.',                          @Now, @Now),
        ('Ferramentas',        'Ferramentas manuais e elétricas para construção e manutenção.',          @Now, @Now),
        ('Papelaria',          'Materiais de escritório, escolar e itens de impressão.',                 @Now, @Now),
        ('Móveis',             'Móveis residenciais e para escritório.',                                  @Now, @Now),
        ('Esportes e Lazer',   'Equipamentos esportivos, artigos de lazer e atividades ao ar livre.',   @Now, @Now);

    -- -------------------------------------------------------
    -- Captura os IDs gerados pela IDENTITY para uso no FK
    -- -------------------------------------------------------
    DECLARE @CatEletronicos       INT = (SELECT Id FROM Categories WHERE Name = 'Eletrônicos');
    DECLARE @CatVestuario         INT = (SELECT Id FROM Categories WHERE Name = 'Vestuário');
    DECLARE @CatAlimentos         INT = (SELECT Id FROM Categories WHERE Name = 'Alimentos');
    DECLARE @CatBebidas           INT = (SELECT Id FROM Categories WHERE Name = 'Bebidas');
    DECLARE @CatHigiene           INT = (SELECT Id FROM Categories WHERE Name = 'Higiene Pessoal');
    DECLARE @CatLimpeza           INT = (SELECT Id FROM Categories WHERE Name = 'Limpeza Doméstica');
    DECLARE @CatFerramentas       INT = (SELECT Id FROM Categories WHERE Name = 'Ferramentas');
    DECLARE @CatPapelaria         INT = (SELECT Id FROM Categories WHERE Name = 'Papelaria');
    DECLARE @CatMoveis            INT = (SELECT Id FROM Categories WHERE Name = 'Móveis');
    DECLARE @CatEsportes          INT = (SELECT Id FROM Categories WHERE Name = 'Esportes e Lazer');

    -- ===========================================================
    --  PRODUTOS — Eletrônicos
    -- ===========================================================
    INSERT INTO Products (Name, Description, Price, CategoryId, CreatedAt, UpdatedAt)
    VALUES
        ('Smartphone Pro 5G',       'Smartphone com tela AMOLED 6.7", 256 GB e câmera tripla de 108 MP.',       2999.90, @CatEletronicos, @Now, @Now),
        ('Notebook Gamer Ultra',    'Notebook com processador i7, 16 GB RAM, SSD 512 GB e GPU RTX 4060.',       5499.00, @CatEletronicos, @Now, @Now),
        ('Tablet 10 Pro',           'Tablet com tela de 10.5", 128 GB, suporte a caneta stylus.',               1299.90, @CatEletronicos, @Now, @Now),
        ('Fone Bluetooth ANC',      'Fone de ouvido over-ear com cancelamento ativo de ruído e 30h de bateria.', 449.90, @CatEletronicos, @Now, @Now),
        ('Monitor 27" 4K',          'Monitor UHD 4K com painel IPS, 144 Hz e entrada USB-C.',                  1899.00, @CatEletronicos, @Now, @Now),
        ('Teclado Mecânico RGB',    'Teclado mecânico com switches Red, retroiluminação RGB e layout ABNT2.',    389.90, @CatEletronicos, @Now, @Now),
        ('Mouse Gamer 25K DPI',     'Mouse óptico com sensor de 25.600 DPI, 7 botões programáveis e RGB.',      219.90, @CatEletronicos, @Now, @Now),
        ('Câmera Mirrorless 4K',    'Câmera com sensor full-frame, gravação 4K 60fps e estabilizador integrado.', 6799.00, @CatEletronicos, @Now, @Now),
        ('Smartwatch Series X',     'Relógio inteligente com monitor cardíaco, GPS e autonomia de 7 dias.',      899.90, @CatEletronicos, @Now, @Now),
        ('SSD NVMe 1TB',            'SSD interno NVMe PCIe 4.0 com velocidade de leitura de até 7.000 MB/s.',   499.00, @CatEletronicos, @Now, @Now);

    -- ===========================================================
    --  PRODUTOS — Vestuário
    -- ===========================================================
    INSERT INTO Products (Name, Description, Price, CategoryId, CreatedAt, UpdatedAt)
    VALUES
        ('Jaqueta Corta-Vento',     'Jaqueta leve impermeável com capuz removível, ideal para atividades externas.', 189.90, @CatVestuario, @Now, @Now),
        ('Calça Jeans Slim',        'Calça jeans masculina corte slim, tecido stretch e lavagem escura.',             129.90, @CatVestuario, @Now, @Now),
        ('Vestido Midi Floral',     'Vestido feminino midi com estampa floral, tecido crepe e alças ajustáveis.',     159.90, @CatVestuario, @Now, @Now),
        ('Tênis Casual Urban',      'Tênis unissex em couro sintético com sola emborrachada antiderrapante.',         249.90, @CatVestuario, @Now, @Now),
        ('Camiseta Dry-Fit UV50',   'Camiseta esportiva com proteção UV50+ e tecnologia de secagem rápida.',          59.90, @CatVestuario, @Now, @Now),
        ('Moletom Oversized',       'Moletom unissex oversized em fleece de algodão com bolso canguru.',              119.90, @CatVestuario, @Now, @Now),
        ('Bermuda Sarja Cargo',     'Bermuda masculina sarja com 6 bolsos e cintura elástica na parte traseira.',      89.90, @CatVestuario, @Now, @Now),
        ('Blusa de Tricot',         'Blusa feminina de tricot com decote V, manga longa e caimento amplo.',           109.90, @CatVestuario, @Now, @Now),
        ('Boné Estruturado',        'Boné aba curva com bordado frontal, fecho de ajuste e interior em algodão.',      49.90, @CatVestuario, @Now, @Now),
        ('Meia Cano Longo Kit 5',   'Kit com 5 pares de meias cano longo em algodão com fio antialérgico.',            39.90, @CatVestuario, @Now, @Now);

    -- ===========================================================
    --  PRODUTOS — Alimentos
    -- ===========================================================
    INSERT INTO Products (Name, Description, Price, CategoryId, CreatedAt, UpdatedAt)
    VALUES
        ('Arroz Branco 5kg',        'Arroz branco tipo 1, longo fino, cozimento rápido.',                            22.90, @CatAlimentos, @Now, @Now),
        ('Feijão Carioca 1kg',      'Feijão carioca selecionado, grão tipo 1, sem impurezas.',                        8.90, @CatAlimentos, @Now, @Now),
        ('Azeite Extra Virgem',     'Azeite de oliva extra virgem 500ml, acidez máxima de 0,5%, importado.',         34.90, @CatAlimentos, @Now, @Now),
        ('Macarrão Espaguete 500g', 'Massa de sêmola de trigo duro, espaguete n.8, cozimento al dente em 9 min.',     5.49, @CatAlimentos, @Now, @Now),
        ('Farinha de Trigo 1kg',    'Farinha de trigo especial, enriquecida com ferro e ácido fólico.',               4.90, @CatAlimentos, @Now, @Now),
        ('Atum em Lata 170g',       'Atum sólido ao natural em lata, sem adição de gordura, alto teor de proteína.',  7.90, @CatAlimentos, @Now, @Now),
        ('Granola Integral 400g',   'Granola com aveia, mel, castanhas e frutas desidratadas, sem adição de açúcar.', 18.90, @CatAlimentos, @Now, @Now),
        ('Molho de Tomate 340g',    'Molho de tomate italiano com manjericão, sem conservantes artificiais.',          4.49, @CatAlimentos, @Now, @Now),
        ('Sardinha em Óleo 125g',   'Sardinha em óleo de soja com adição de sal, rico em ômega 3.',                   5.90, @CatAlimentos, @Now, @Now),
        ('Aveia em Flocos 500g',    'Aveia em flocos finos, fonte de fibras e carboidratos de liberação lenta.',       9.90, @CatAlimentos, @Now, @Now);

    -- ===========================================================
    --  PRODUTOS — Bebidas
    -- ===========================================================
    INSERT INTO Products (Name, Description, Price, CategoryId, CreatedAt, UpdatedAt)
    VALUES
        ('Água Mineral 500ml Cx24', 'Caixa com 24 garrafas de água mineral natural sem gás de 500 ml.',              29.90, @CatBebidas, @Now, @Now),
        ('Suco de Laranja 1L',      'Suco de laranja integral, 100% natural, sem adição de açúcar ou conservantes.', 12.90, @CatBebidas, @Now, @Now),
        ('Refrigerante Cola 2L',    'Refrigerante sabor cola 2L, versão original com gás.',                           9.90, @CatBebidas, @Now, @Now),
        ('Energético 473ml',        'Bebida energética com taurina, cafeína e vitaminas do complexo B.',              10.90, @CatBebidas, @Now, @Now),
        ('Cerveja Pilsen Lata 350ml','Cerveja pilsen lata 350ml, teor alcoólico 4,7%, temperatura ideal 0-4°C.',       4.49, @CatBebidas, @Now, @Now),
        ('Vinho Tinto Seco 750ml',  'Vinho tinto seco brasileiro, blend de Cabernet e Merlot, safra 2022.',           49.90, @CatBebidas, @Now, @Now),
        ('Café Torrado Moído 500g', 'Café 100% arábica, torra média, moagem fina para espresso.',                    24.90, @CatBebidas, @Now, @Now),
        ('Chá Verde cx 20 sachês',  'Chá verde com antioxidantes naturais, sem cafeína, sabor natural.',               8.90, @CatBebidas, @Now, @Now),
        ('Leite UHT Integral 1L',   'Leite longa vida integral, enriquecido com vitaminas A e D.',                    5.90, @CatBebidas, @Now, @Now),
        ('Isotônico Citrus 500ml',  'Bebida isotônica sabor citrus com eletrólitos para reposição hídrica.',           6.90, @CatBebidas, @Now, @Now);

    -- ===========================================================
    --  PRODUTOS — Higiene Pessoal
    -- ===========================================================
    INSERT INTO Products (Name, Description, Price, CategoryId, CreatedAt, UpdatedAt)
    VALUES
        ('Shampoo Hidratação 400ml','Shampoo com queratina e óleo de argan para cabelos danificados e ressecados.',   19.90, @CatHigiene, @Now, @Now),
        ('Condicionador 400ml',     'Condicionador nutritivo com proteínas do leite e vitamina E.',                   19.90, @CatHigiene, @Now, @Now),
        ('Sabonete Líquido 250ml',  'Sabonete líquido antibacteriano com extrato de aloe vera e pH neutro.',           9.90, @CatHigiene, @Now, @Now),
        ('Creme Dental Branqueador','Creme dental com flúor 1450ppm, branqueador e proteção anticárie 12h.',          12.90, @CatHigiene, @Now, @Now),
        ('Desodorante Roll-On',     'Desodorante antitranspirante roll-on com proteção de 72h, sem álcool.',          14.90, @CatHigiene, @Now, @Now),
        ('Protetor Solar FPS60',    'Protetor solar facial FPS 60, toque seco, resistente à água, 60ml.',            39.90, @CatHigiene, @Now, @Now),
        ('Aparelho de Barbear 5L',  'Aparelho de barbear com 5 lâminas, faixa lubrificante e cabeçote flexível.',    29.90, @CatHigiene, @Now, @Now),
        ('Fio Dental 50m',          'Fio dental encerado com menta, caixa com 50 metros.',                            5.90, @CatHigiene, @Now, @Now),
        ('Hidratante Corporal 400ml','Loção hidratante corporal com manteiga de karité, absorção rápida.',            22.90, @CatHigiene, @Now, @Now),
        ('Escova de Dente Macia',   'Escova dental com cerdas macias e cabo ergonômico antiderrapante.',               7.90, @CatHigiene, @Now, @Now);

    -- ===========================================================
    --  PRODUTOS — Limpeza Doméstica
    -- ===========================================================
    INSERT INTO Products (Name, Description, Price, CategoryId, CreatedAt, UpdatedAt)
    VALUES
        ('Detergente Neutro 500ml', 'Detergente líquido neutro, biodegradável, para louças e superfícies delicadas.',  3.49, @CatLimpeza, @Now, @Now),
        ('Desinfetante Lavanda 2L', 'Desinfetante bactericida e fungicida com fragrância de lavanda, diluível.',       9.90, @CatLimpeza, @Now, @Now),
        ('Água Sanitária 1L',       'Água sanitária 2,0-2,5% cloro ativo, para limpeza e desinfecção geral.',          5.90, @CatLimpeza, @Now, @Now),
        ('Sabão em Pó 1,5kg',       'Sabão em pó multiação com enzimas para remoção de manchas difíceis.',            18.90, @CatLimpeza, @Now, @Now),
        ('Amaciante Concentrado 1L','Amaciante concentrado, fragrância floral, até 40 lavagens por litro.',           12.90, @CatLimpeza, @Now, @Now),
        ('Esponja Dupla Face cx5',  'Caixa com 5 esponjas dupla-face, lado abrasivo e macio, 110x75mm.',               8.90, @CatLimpeza, @Now, @Now),
        ('Limpador Multiuso 500ml', 'Limpador multiuso em spray para cozinha, banheiro e superfícies diversas.',       7.90, @CatLimpeza, @Now, @Now),
        ('Pano de Chão Microfibra', 'Pano de chão em microfibra de alta absorção, lavável, 60x40cm.',                 14.90, @CatLimpeza, @Now, @Now),
        ('Rodo com Cabo 1,40m',     'Rodo profissional com cabo telescópico de 1,40m e borracha dupla.',              29.90, @CatLimpeza, @Now, @Now),
        ('Balde Plástico 15L',      'Balde plástico resistente 15 litros com alça metálica reforçada.',               19.90, @CatLimpeza, @Now, @Now);

    -- ===========================================================
    --  PRODUTOS — Ferramentas
    -- ===========================================================
    INSERT INTO Products (Name, Description, Price, CategoryId, CreatedAt, UpdatedAt)
    VALUES
        ('Furadeira/Parafusadeira', 'Furadeira de impacto 850W, mandril 13mm, com maleta e jogo de brocas.',        289.90, @CatFerramentas, @Now, @Now),
        ('Serra Circular 7.1/4"',   'Serra circular 1.400W, disco 7.1/4" e guia paralela ajustável.',               419.90, @CatFerramentas, @Now, @Now),
        ('Jogo de Chaves Allen',    'Jogo com 9 chaves allen em aço Cr-V, tamanhos 1,5mm a 10mm, em estojo.',        34.90, @CatFerramentas, @Now, @Now),
        ('Alicate Universal 8"',    'Alicate universal 8 polegadas, cabo bicomponente isolado até 1000V.',           39.90, @CatFerramentas, @Now, @Now),
        ('Trena Digital 50m',       'Trena a laser com alcance de 50m, precisão ±1,5mm e display retroiluminado.',   89.90, @CatFerramentas, @Now, @Now),
        ('Nível de Bolha 60cm',     'Nível de alumínio 60cm com 3 ampolas e base magnética.',                        44.90, @CatFerramentas, @Now, @Now),
        ('Marreta 1kg',             'Marreta de aço forjado 1kg com cabo em madeira de eucalipto tratado.',          49.90, @CatFerramentas, @Now, @Now),
        ('Caixa de Ferramentas 40cm','Caixa organizadora em polipropileno 40cm com bandeja removível.',              79.90, @CatFerramentas, @Now, @Now),
        ('Fita Isolante Cx10',      'Caixa com 10 rolos de fita isolante 19mm x 10m, antichama 70°C.',              29.90, @CatFerramentas, @Now, @Now),
        ('Estilete Retrátil 18mm',  'Estilete profissional com lâmina de 18mm, trava metálica e grip emborrachado.', 19.90, @CatFerramentas, @Now, @Now);

    -- ===========================================================
    --  PRODUTOS — Papelaria
    -- ===========================================================
    INSERT INTO Products (Name, Description, Price, CategoryId, CreatedAt, UpdatedAt)
    VALUES
        ('Caneta Esferográfica cx10','Caixa com 10 canetas esferográficas azuis, ponta 0.7mm, escrita suave.',         9.90, @CatPapelaria, @Now, @Now),
        ('Caderno Universitário 96f','Caderno universitário capa dura 96 folhas, formato A4, papel 90g.',             24.90, @CatPapelaria, @Now, @Now),
        ('Lápis de Cor 24 cores',   'Lápis de cor com 24 cores vibrantes, madeira resinada e grafite resistente.',   19.90, @CatPapelaria, @Now, @Now),
        ('Post-it 76x76mm cx12',    'Caixa com 12 blocos de 100 folhas autoadesivas, cores variadas.',               29.90, @CatPapelaria, @Now, @Now),
        ('Grampeador 26/6',         'Grampeador de mesa até 30 folhas, grampo 26/6, base antiderrapante.',            22.90, @CatPapelaria, @Now, @Now),
        ('Papel A4 Resma 500fls',   'Resma de papel A4 500 folhas, 75g/m², brancura 91% ISO, livre de ácido.',       39.90, @CatPapelaria, @Now, @Now),
        ('Marcador de Texto cx4',   'Caixa com 4 marcadores de texto fluorescentes, ponta chanfrada 1-5mm.',          9.90, @CatPapelaria, @Now, @Now),
        ('Pasta Suspensa cx25',     'Caixa com 25 pastas suspensas kraft com visor e talão de identificação.',        49.90, @CatPapelaria, @Now, @Now),
        ('Calculadora Científica',  'Calculadora científica com 240 funções, display de 10+2 dígitos.',              54.90, @CatPapelaria, @Now, @Now),
        ('Corretivo Líquido 18ml',  'Corretivo líquido base água com pincel de pelo fino, secagem em 10 segundos.',   4.90, @CatPapelaria, @Now, @Now);

    -- ===========================================================
    --  PRODUTOS — Móveis
    -- ===========================================================
    INSERT INTO Products (Name, Description, Price, CategoryId, CreatedAt, UpdatedAt)
    VALUES
        ('Cadeira Gamer Reclinável', 'Cadeira gamer com suporte lombar, apoio de braços 4D e reclinação 180°.',      899.90, @CatMoveis, @Now, @Now),
        ('Mesa de Escritório 1,50m', 'Mesa reta 150x60cm em MDF 25mm, pés metálicos, suporta até 60kg.',             499.00, @CatMoveis, @Now, @Now),
        ('Estante 5 Prateleiras',   'Estante modular MDF 5 prateleiras 180x80cm, fácil montagem, suporta 20kg/prat.',399.90, @CatMoveis, @Now, @Now),
        ('Sofá 3 Lugares Veludo',   'Sofá retrátil e reclinável 3 lugares em veludo canelado, pés em madeira.',     1299.00, @CatMoveis, @Now, @Now),
        ('Rack para TV 55"',        'Rack painel suspenso 160cm com suporte para TV até 55", 2 portas com LED.',     649.90, @CatMoveis, @Now, @Now),
        ('Cama Box Casal 138x188',  'Cama box casal com colchão D33, altura 52cm, 5 anos de garantia.',            1199.00, @CatMoveis, @Now, @Now),
        ('Guarda-Roupa 6 Portas',   'Guarda-roupa 6 portas com espelho, 2 gavetas e cabideiro duplo, 220x52x185cm.',899.00, @CatMoveis, @Now, @Now),
        ('Escrivaninha Compacta',   'Escrivaninha 90x45cm com gaveta, ideal para home office em espaços reduzidos.', 299.90, @CatMoveis, @Now, @Now),
        ('Banqueta Alta Bar cx2',   'Par de banquetas altas para balcão, assento estofado e base giratória em aço.', 349.90, @CatMoveis, @Now, @Now),
        ('Mesa de Centro Vidro',    'Mesa de centro com tampo em vidro temperado 8mm e estrutura em aço cromado.',   399.00, @CatMoveis, @Now, @Now);

    -- ===========================================================
    --  PRODUTOS — Esportes e Lazer
    -- ===========================================================
    INSERT INTO Products (Name, Description, Price, CategoryId, CreatedAt, UpdatedAt)
    VALUES
        ('Bicicleta MTB Aro 29',    'Mountain bike aro 29, quadro alumínio, 21 marchas, freio a disco mecânico.',   1499.00, @CatEsportes, @Now, @Now),
        ('Halteres Emborrachados 2x10kg','Par de halteres emborrachados 10kg, pegada antiderrapante, hexagonal.',    199.90, @CatEsportes, @Now, @Now),
        ('Esteira Elétrica 12km/h', 'Esteira elétrica com velocidade máx 12km/h, 12 programas e display digital.',  1299.00, @CatEsportes, @Now, @Now),
        ('Bola de Futebol Campo',   'Bola de futebol de campo costurada, revestimento PU termobondado, tamanho 5.',   79.90, @CatEsportes, @Now, @Now),
        ('Raquete de Beach Tennis', 'Raquete de beach tennis em fibra de vidro com capa protetora, peso 360g.',      189.90, @CatEsportes, @Now, @Now),
        ('Tapete de Yoga 6mm',      'Tapete de yoga antiderrapante 180x60cm, espessura 6mm, com alça de transporte.', 59.90, @CatEsportes, @Now, @Now),
        ('Corda de Pular Speed',    'Corda de pular profissional com rolamentos de alta velocidade, cabo de aço.',    44.90, @CatEsportes, @Now, @Now),
        ('Luvas de Boxe 14oz',      'Luvas de boxe couro sintético 14oz com velcro, espuma de alta densidade.',      149.90, @CatEsportes, @Now, @Now),
        ('Mochila de Hidratação 15L','Mochila trail running 15L com reservatório 2L e sistema de ventilação dorsal.', 179.90, @CatEsportes, @Now, @Now),
        ('Prancha de Stand-Up Paddle','SUP inflável 10''6" com remo ajustável, bomba, mochila e kit de reparo.',     999.00, @CatEsportes, @Now, @Now);

    COMMIT TRANSACTION;
    PRINT 'Seeder concluído: 10 categorias e 100 produtos inseridos com sucesso.';

END TRY
BEGIN CATCH
    ROLLBACK TRANSACTION;
    PRINT 'Erro ao executar o seeder: ' + ERROR_MESSAGE();
    THROW;
END CATCH;
