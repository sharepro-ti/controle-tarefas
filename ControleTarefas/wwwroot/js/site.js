"use strict";

const quadro = document.getElementById("quadro");

if (quadro) {
	const formulario = document.getElementById("formulario-tarefa");
	const modalElemento = document.getElementById("modal-tarefa");
	const modal = bootstrap.Modal.getOrCreateInstance(modalElemento);
	const salvar = document.getElementById("salvar-tarefa");
	const erroCadastro = document.getElementById("erro-cadastro");
	const erroQuadro = document.getElementById("erro-quadro");
	let salvando = false;
	let movendo = false;
	let cartaoArrastado = null;

	function atualizarColunas() {
		quadro.querySelectorAll(".coluna").forEach(coluna => {
			const quantidade = coluna.querySelectorAll(".cartao").length;
			coluna.querySelector(".contador").textContent = quantidade;
			coluna.querySelector(".coluna-vazia").hidden = quantidade > 0;
		});
		document.getElementById("total-tarefas").textContent = quadro.querySelectorAll(".cartao").length;
	}

	function mostrarErro(elemento, mensagem) {
		elemento.textContent = mensagem;
		elemento.hidden = false;
	}

	async function verificarResposta(resposta) {
		if (!resposta.ok) {
			const mensagem = await resposta.text();
			throw new Error(resposta.status >= 500 || mensagem.trim().startsWith("<") || !mensagem
				? "Não foi possível salvar. Tente novamente."
				: mensagem);
		}
	}

	modalElemento.addEventListener("shown.bs.modal", () => formulario.querySelector("input[name='Tarefa.Titulo']").focus());
	modalElemento.addEventListener("hide.bs.modal", evento => {
		if (salvando) evento.preventDefault();
	});
	modalElemento.addEventListener("hidden.bs.modal", () => {
		formulario.reset();
		erroCadastro.hidden = true;
	});

	formulario.addEventListener("submit", async evento => {
		evento.preventDefault();
		if (salvando || !formulario.reportValidity()) return;

		salvando = true;
		salvar.disabled = true;
		salvar.textContent = "Salvando...";
		erroCadastro.hidden = true;
		const dados = new FormData(formulario);
		const campos = formulario.querySelectorAll("input, textarea, select, button");
		campos.forEach(campo => campo.disabled = true);

		try {
			const resposta = await fetch(formulario.action, { method: "POST", body: dados });
			await verificarResposta(resposta);
			const fragmento = document.createElement("template");
			fragmento.innerHTML = await resposta.text();
			const cartao = fragmento.content.querySelector(".cartao");
			const coluna = cartao && quadro.querySelector(`.coluna[data-status="${cartao.dataset.status}"] .coluna-cartoes`);
			if (!coluna) throw new Error("Não foi possível receber a tarefa salva. Recarregue o quadro.");

			const proximo = Array.from(coluna.querySelectorAll(".cartao")).find(existente =>
				existente.querySelector("time").dateTime > cartao.querySelector("time").dateTime);
			coluna.insertBefore(cartao, proximo || coluna.querySelector(".coluna-vazia"));
			atualizarColunas();
			salvando = false;
			modal.hide();
		} catch (erro) {
			mostrarErro(erroCadastro, erro instanceof TypeError
				? "Não foi possível conectar ao servidor. Tente novamente."
				: erro.message);
		} finally {
			salvando = false;
			campos.forEach(campo => campo.disabled = false);
			salvar.textContent = "Salvar tarefa";
		}
	});

	quadro.addEventListener("dragstart", evento => {
		const cartao = evento.target.closest(".cartao");
		if (!cartao || movendo) {
			evento.preventDefault();
			return;
		}
		cartaoArrastado = cartao;
		evento.dataTransfer.setData("text/plain", cartao.dataset.id);
		evento.dataTransfer.effectAllowed = "move";
		cartao.classList.add("arrastando");
	});

	quadro.addEventListener("dragend", () => {
		cartaoArrastado?.classList.remove("arrastando");
		cartaoArrastado = null;
		quadro.querySelectorAll(".destino").forEach(coluna => coluna.classList.remove("destino"));
	});

	quadro.addEventListener("dragover", evento => {
		const coluna = evento.target.closest(".coluna");
		if (!coluna || !cartaoArrastado || movendo) return;
		evento.preventDefault();
		evento.dataTransfer.dropEffect = "move";
		quadro.querySelectorAll(".destino").forEach(destino => destino.classList.remove("destino"));
		coluna.classList.add("destino");
	});

	quadro.addEventListener("dragleave", evento => {
		const coluna = evento.target.closest(".coluna");
		if (coluna && !coluna.contains(evento.relatedTarget)) coluna.classList.remove("destino");
	});

	quadro.addEventListener("drop", async evento => {
		evento.preventDefault();
		const coluna = evento.target.closest(".coluna");
		const cartao = cartaoArrastado;
		if (!coluna || !cartao || movendo || cartao.dataset.status === coluna.dataset.status) return;

		movendo = true;
		erroQuadro.hidden = true;
		coluna.classList.remove("destino");
		const dados = new FormData();
		dados.append("id", cartao.dataset.id);
		dados.append("status", coluna.dataset.status);
		dados.append("__RequestVerificationToken", formulario.querySelector("input[name='__RequestVerificationToken']").value);

		try {
			const resposta = await fetch(quadro.dataset.moverUrl, { method: "POST", body: dados });
			await verificarResposta(resposta);
			const destino = coluna.querySelector(".coluna-cartoes");
			const proximo = Array.from(destino.querySelectorAll(".cartao")).find(existente => {
				const dataExistente = existente.querySelector("time").dateTime;
				const dataCartao = cartao.querySelector("time").dateTime;
				return dataExistente > dataCartao || (dataExistente === dataCartao && Number(existente.dataset.id) > Number(cartao.dataset.id));
			});
			destino.insertBefore(cartao, proximo || coluna.querySelector(".coluna-vazia"));
			cartao.dataset.status = coluna.dataset.status;
			atualizarColunas();
		} catch (erro) {
			mostrarErro(erroQuadro, erro instanceof TypeError
				? "Não foi possível conectar ao servidor. Tente novamente."
				: erro.message);
		} finally {
			movendo = false;
		}
	});
}
