### Prefabs
- Nome
- Descrição
- Quando são utilizados
- Quais seus componentes
    - Sprites
    - Colisores
    - Fontes de audio
    - Scripts
        - descreva o comportamento dos scripts

Nome: Vex
Descrição: Player
Quando são utilizados: Durante todo o jogo
Componentes:
	- Sprites: spritesheet com animações idle, walk, attack
	- Colliders: RigidBody 2D (kinematic), Capsule Collider 2D
	- Fontes de Audio: Sim
	- Scripts: controle através do teclado e mouse para movimentação, ataque e interações. Seleção de poção.

Nome: Golem
Descrição: NPC que guia o usuário. Fica parado, porém o player pode interagir.
Quando são utilizados: Quando o player entra na área de interação com ele na base
Componentes:
	- Sprites: spritesheet de movimento idle.
	- Colliders: Box Collider 2D com Trigger ativado
	- Fontes de Audio: Sim
	- Scripts (descrição): script de fala; altera o conteúdo de acordo com a conclusão de fases para guiar o player

Nome: Mestre Alquimista
Descrição: boss final do jogo
Quando são utilizados: ao chegar na última fase do jogo
Componentes:
	- Sprites: spritesheet de animação com idle, walk, e ataque
	- Colliders: RigidBody 2D, Capsule Collider 2D com Trigger ativado
	- Fontes de Audio: sim
	- Scripts: andar e parar em posições especificas antes de ver o player. Quando o player entra em sua zona, ele anda até o player e ataca (bate). Se acertar (Trigger detectar o player) o player deve tomar dano. Se ele for acertado pelo player, deve tomar dano. Ao perder todas as vidas, morre, ou seja, o GameObject é deletado. Quando a vida está menor que a metade, entra em sua segunda fase: aumenta sua velocidade, dá 2 de dano e inclui um novo ataque (pula e ataca).

Nome: zumbis
Descrição: principais inimigos do jogo
Quando são utilizados: ao spawnar inimigos no inicio do jogo e em cada fase (dungeon)
Componentes:
	- Sprites: spritesheet de animação de idle, walk e combate. 2 tipos de visuais.
	- Colliders: RigidBody 2D, Capsule Collider 2D com Trigger ativado
	- Fontes de Audio: Sim
	- Scripts: andar e parar em posições especificas antes de ver o player. Quando o player entra em sua zona, ele anda até o player e ataca (bate). Se acertar (Trigger detectar o player) o player deve tomar dano. Se ele for acertado pelo player, deve tomar dano. Ao perder todas as vidas, morre, ou seja, o GameObject é deletado.

Nome: Paciente zero
Descrição: Chefe secundario
Quando são utilizados: quando o player entrar na sala do chefe da segunda dungeon
Componentes:
	- Sprites: spritesheet de animação de idle, walk e combate.
	- Colliders: RigidBody 2D, Capsule Collider 2D com Trigger ativado
	- Fontes de Audio: Sim
    - Scripts: andar e parar, andar até o player e atacar (bate) se dentro de seu campo de visão, tomar dano e morrer, menos de metade da vida ativa 2 fase: mais rápido

Nome: Poções
Descrição: Poções que causam efeitos nos inimigos
	- Gelo (congela inimigos), Fogo (causa dano), Raio (dano em sequência) e Explosão (dano em área)
Quando são utilizados: ao utilizar o ataque do player (ataque funciona apenas com a poção selecionada)
Componentes:
	- Sprites: spritesheets de animação para cada tipo de poção
	- Colliders: Box Collider 2D com Trigger ativado
	- Fontes de Audio: sim
	- Scripts: arremesso com range variável para cada tipo de poção, dano ao inimigo ao colidir, outros efeitos adversos: 
		- gelo: imobiliza inimigos dirante 3s
		- fogo: dano base
		- explosão: collider maior de forma a causar dano em área
		- raio: ao colidir com um inimigo, atingirá outros em sequencia a partir de uma distância x do alvo atingido

Nome: Vida
Descrição: corações que medem quantas vidas o Player tem 
Quando são utilizados: Toda vez que o Player toma dano ou ganha vida eles devem ser alterados
Componentes:
	- Sprites: poção de coração cheio e vazio
	- Colliders: Não
	- Fontes de Audio: Não
	- Scripts: criação visual do numero de corações correspondente à vida do Player - dinâmico, deve ser alterado conforme as vidas do player.

Nome: itens
Descrição: itens coletáveis do jogo (ingredientes do elixir da vida, poções de restauração de vida, armadura para vida extra) e itens de ativação de habilidade
Quando são utilizados: espalhados pela dungeon e ao completar a dungeon.
Componentes:
	- Sprites: imagem do objeto e iluminação para brilho.
	- Colliders: Collider 2D com trigger ativado
	- Fontes de Audio: não
	- Scripts: ao colidir com o player, devem desaparecer. Devem flutuar.
        se for ingrediente: entra para contagem de itens do elixir da vida
        se forem itens de vida (vida extra ou armadura): aumentam a vida atual ou máxima do player
        se forem coletáveis de poção ou bota: desbloqueia, habilidades
