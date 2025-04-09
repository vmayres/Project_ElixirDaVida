# 🧍‍♂️ Atores do Jogo

## 🎮 Vex (Player)
**Descrição:** Personagem jogável.  
**Quando é utilizado:** Durante todo o jogo.

**Componentes:**
- **Sprites:** Spritesheet com animações de idle, walk, attack.
- **Colliders:** RigidBody 2D (kinematic), Capsule Collider 2D.
- **Fontes de Áudio:** Sim.
- **Scripts:** Controle por teclado e mouse para movimentação, ataque e interações. Seleção de poção.

---

## 🪨 Golem (NPC)
**Descrição:** NPC que guia o jogador. Permanece parado, mas interage com o player.  
**Quando é utilizado:** Quando o jogador entra na área de interação com ele na base.

**Componentes:**
- **Sprites:** Spritesheet de animação idle.
- **Colliders:** Box Collider 2D com Trigger ativado.
- **Fontes de Áudio:** Sim.
- **Scripts:** Script de fala; altera o conteúdo conforme o progresso do jogador nas fases.

---

## 🧪 Mestre Alquimista (Boss Final)
**Descrição:** Boss final do jogo.  
**Quando é utilizado:** Ao chegar na última fase do jogo.

**Componentes:**
- **Sprites:** Spritesheet com animações de idle, walk, ataque.
- **Colliders:** RigidBody 2D, Capsule Collider 2D com Trigger ativado.
- **Fontes de Áudio:** Sim.
- **Scripts:**  
  - Anda até posições específicas antes de ver o player.  
  - Ao detectar o player, anda até ele e ataca.  
  - Se acerta o player, causa dano.  
  - Se for acertado, toma dano.  
  - Ao atingir metade da vida, entra na segunda fase:  
    - Aumenta velocidade.  
    - Ataques causam 2 de dano.  
    - Ganha novo ataque (salta e ataca).  
  - Ao perder toda a vida:
    - Se usado elixir: fica vivo e vai para cutscene. 
    - Se não usado elixir: morre e vai para cutscene. 
---

## 🧟‍♂️ Zumbis
**Descrição:** Inimigos comuns.  
**Quando são utilizados:** Durante todo o jogo e em todas as fases (dungeons).

**Componentes:**
- **Sprites:** Spritesheet com animações de idle, walk e combate (2 tipos visuais).
- **Colliders:** RigidBody 2D, Capsule Collider 2D com Trigger ativado.
- **Fontes de Áudio:** Sim.
- **Scripts:**  
  - Patrulham até detectar o player.  
  - Correm e atacam o player.  
  - Ao acertar, causam dano.  
  - Ao serem atingidos, perdem vida.  
  - Ao morrer, são destruídos.

---

## ☣️ Paciente Zero (Mini-Boss)
**Descrição:** Chefe secundário.  
**Quando é utilizado:** Ao entrar na sala do chefe da segunda dungeon.

**Componentes:**
- **Sprites:** Spritesheet com animações de idle, walk e combate.
- **Colliders:** RigidBody 2D, Capsule Collider 2D com Trigger ativado.
- **Fontes de Áudio:** Sim.
- **Scripts:**  
  - Patrulha e detecta o player.  
  - Corre e ataca.  
  - Ao ser atingido, toma dano.  
  - Quando vida < 50%, entra na 2ª fase (mais rápido e agressivo).  
  - Ao morrer, é destruído.

---

## 🧪 Poções
**Descrição:** Poções mágicas com efeitos únicos.  
**Tipos:**  
- Gelo (congela inimigos)  
- Fogo (causa dano direto)  
- Raio (dano em cadeia)  
- Explosão (dano em área)

**Quando são utilizadas:** Ao atacar com a poção selecionada.

**Componentes:**
- **Sprites:** Spritesheets animadas para cada tipo.
- **Colliders:** Box Collider 2D com Trigger ativado.
- **Fontes de Áudio:** Sim.
- **Scripts:**  
  - Arremesso com range específico por tipo.  
  - Causam dano e efeitos distintos:  
    - **Gelo:** Congela por 3s.  
    - **Fogo:** Dano base.  
    - **Explosão:** Área maior de dano.  
    - **Raio:** Dano em cadeia em inimigos próximos.

---

## ❤️ Vida (UI)
**Descrição:** Corações que representam a vida do jogador.  
**Quando são utilizados:** Sempre que o player toma ou recupera dano.

**Componentes:**
- **Sprites:** Coração cheio e vazio.
- **Colliders:** Não.
- **Fontes de Áudio:** Não.
- **Scripts:** Atualização dinâmica dos corações exibidos na UI conforme a vida atual do jogador.

---

## 🎁 Itens Coletáveis
**Descrição:**  
- Ingredientes do Elixir da Vida  
- Poções de cura  
- Equipamentos (peitoral, bota)  
- Itens de habilidade

**Quando são utilizados:** Espalhados pelas dungeons e após completá-las.

**Componentes:**
- **Sprites:** Imagem do item + brilho/iluminação.
- **Colliders:** Collider 2D com Trigger ativado.
- **Fontes de Áudio:** Não.
- **Scripts:**  
  - Ao colidir com o player, somem.  
  - **Efeitos:**  
    - **Ingrediente:** entra na contagem do Elixir.  
    - **Vida extra/armadura:** aumenta vida atual ou máxima.  
    - **Bota/poção especial:** desbloqueia nova habilidade.
  - Devem ter animação de flutuar no cenário.

---

