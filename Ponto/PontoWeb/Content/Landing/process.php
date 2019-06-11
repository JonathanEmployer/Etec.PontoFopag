<?php
// Configure your Subject Prefix and Recipient here
$subjectPrefix = '[Cwork Ponto Web]';
$emailTo       = '<vendas@cwork.com.br>';

$errors = array(); // array to hold validation errors
$data   = array(); // array to pass back data

if($_SERVER['REQUEST_METHOD'] === 'POST') {
    $name    = stripslashes(trim($_POST['name']));
    $email   = stripslashes(trim($_POST['email']));
    
    $message = stripslashes(trim($_POST['message']));
    $telefone = stripslashes(trim($_POST['telefone']));


    if (empty($name)) {
        $errors['name'] = 'Por favor, digite seu nome.';
    }

    if (!preg_match('/^[^0-9][A-z0-9._%+-]+([.][A-z0-9_]+)*[@][A-z0-9_]+([.][A-z0-9_]+)*[.][A-z]{2,4}$/', $email)) {
        $errors['email'] = 'Ops, este e-mail é inválido.';
    }

   

    if (empty($message)) {
        $errors['message'] = 'Por favor, escreva sua mensagem.';
    }

    // if there are any errors in our errors array, return a success boolean or false
    if (!empty($errors)) {
        $data['success'] = false;
        $data['errors']  = $errors;
    } else {
        $subject = "$subjectPrefix";
        $body    = '
            <strong>Nome: </strong>'.$name.'<br />
            <strong>Email: </strong>'.$email.'<br />
            <strong>Telefone: </strong>'.$telefone.'<br />
            <strong>Mensagem: </strong>'.nl2br($message).'<br />
        ';

        $headers  = 'MIME-Version: 1.1' . PHP_EOL;
        $headers .= 'Content-type: text/html; charset=UTF-8' . PHP_EOL;
        $headers .= "From: $name <$email>" . PHP_EOL;
        $headers .= "Return-Path: $emailTo" . PHP_EOL;
        $headers .= "Reply-To: $email" . PHP_EOL;
        $headers .= "X-Mailer: PHP/". phpversion() . PHP_EOL;

        mail($emailTo, $subject, $body, $headers);

        $data['success'] = true;
        $data['message'] = 'Muito Obrigado. Sua mensagem foi enviada com sucesso. Retornamos em breve.';
    }

    // return all our data to an AJAX call
    echo json_encode($data);
}